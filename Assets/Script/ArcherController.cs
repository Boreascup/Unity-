using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    Animator animator;
    bool isPulling = false;
    float pullDuration = 0f;
    public float maxPullDuration = 2f; // 最长拉弓时间
    public float arrowSpeed = 1f; // 箭矢速度
    float pullStrength;
    public Transform firePoint; // 箭矢发射点

    public Camera maincam; // 主摄像机
    public bool isAiming = false; // 是否正在瞄准
    public float normalFOV = 60f; // 普通视角
    public float aimFOV = 30f; // 瞄准时视角
    public float aimTransitionSpeed = 5f; // 视角平滑过渡速度

    public int area1Shots = 10;
    public int area2Shots = 10;

    public AudioClip arrowShootSound; // 射箭音效
    private AudioSource audioSource;

    void Start()
    {
        // 获取弓上的Animator组件
        animator = GetComponent<Animator>();
        // 播放音效
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        // 右键按下，开启瞄准
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
        }
        // 右键松开，取消瞄准
        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        // 平滑过渡视角
        if (maincam != null)
        {
            float targetFOV = isAiming ? aimFOV : normalFOV;
            maincam.fieldOfView = Mathf.Lerp(maincam.fieldOfView, targetFOV, aimTransitionSpeed * Time.deltaTime);
        }

        // 检查是否在允许射击的范围内
        if (IsInArea())
        {
            // 按住空格键：拉弓（Fill 状态）
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pullDuration = 0;
                isPulling = true;
                animator.SetBool("isPulling", true);
                animator.SetBool("isShooting", false);
                animator.SetBool("isHolding", false);
            }

            // 持续按住空格键计算拉力
            if (isPulling)
            {
                pullDuration += Time.deltaTime;
                pullStrength = Mathf.Clamp01(pullDuration / maxPullDuration);
                animator.SetFloat("pullStrength", pullStrength); // 更新Animator中的参数
            }

            // 松开空格键：保持当前拉力状态（Hold 状态）
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isPulling = false;
                animator.SetBool("isPulling", false);
                animator.SetBool("isHolding", true);
            }

            // 按下鼠标左键：发射箭矢（Shoot 状态）
            if (Input.GetMouseButtonDown(0) && animator.GetBool("isHolding"))
            {
                animator.SetBool("isShooting", true);
                animator.SetBool("isHolding", false);

                // 动画事件触发箭矢发射
            }
        }
    }


    /// <summary>
    /// 发射箭矢，绑定到Shoot动画的事件
    /// </summary>
    public void FireArrow()
    {
        // 检查射箭区域
        Vector3 currentPosition = transform.position;
        Vector3 area1 = new Vector3(489f, 46f, 593f);
        Vector3 area2 = new Vector3(666f, 4f, 343f);
        float radius = 10f;

        bool inArea1 = Vector3.Distance(currentPosition, area1) <= radius;
        bool inArea2 = Vector3.Distance(currentPosition, area2) <= radius;

        if (inArea1 && area1Shots > 0)
        {
            area1Shots--; // 减少区域1箭数
            Debug.Log($"区域1剩余箭数：{area1Shots}");
        }
        else if (inArea2 && area2Shots > 0)
        {
            area2Shots--; // 减少区域2箭数
            Debug.Log($"区域2剩余箭数：{area2Shots}");
        }
        else
        {
            Debug.LogWarning("当前区域无箭数可用！");
            return;
        }

        // 创建箭矢对象
        GameObject arrow = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Arrow"));
        arrow.AddComponent<ArrowController>();
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        arrowController.hitSound = Resources.Load<AudioClip>("Sound/hit");
        arrowController.cam = maincam;

        arrow.transform.position = firePoint.transform.position;
        arrow.transform.rotation = Quaternion.LookRotation(this.transform.forward);

        Rigidbody rd = arrow.GetComponent<Rigidbody>();
        if (rd != null)
        {
            rd.AddForce(this.transform.forward * 1 * pullStrength, ForceMode.Impulse);

            // 播放射箭音效
            if (arrowShootSound != null)
            {
                audioSource.PlayOneShot(arrowShootSound);
                Debug.Log($"播放了射箭音效");
            }
        }


        // 动画回到Empty状态
        animator.SetBool("isShooting", false);
    }


    /// <summary>
    /// 检查是否在允许射箭的区域
    /// </summary>
    /// <returns></returns>
    bool IsInArea()
    {
        Vector3 currentPosition = transform.position;
        Vector3 area1 = new Vector3(489f, 46f, 593f);
        Vector3 area2 = new Vector3(666f, 4f, 343f);
        float distance1 = Vector3.Distance(currentPosition, area1);
        float distance2 = Vector3.Distance(currentPosition, area2);
        float radius = 10f;

        if (distance1 <= radius || distance2 <= radius)
        {
            return true;
        }
        else
        {
            Debug.Log("不在射箭范围内！");
            return false;
        }
    }
}
