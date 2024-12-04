using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public float Score = 0;

    public Camera cam;
    private UserGui ui;
    public float hitDisplayTime = 2f; // 提示显示时间
    public ArrowController archerControllerScript;

    public AudioClip hitSound; // 撞击音效
    private AudioSource audioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ui = cam.GetComponent<UserGui>();
        archerControllerScript = GameObject.FindObjectOfType<ArrowController>();
        audioSource = GetComponent<AudioSource>(); // 获取 AudioSource 组件
                                                   
        hitSound = Resources.Load<AudioClip>("Sound/hit"); // 确保音效文件路径正确
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("staticTarget"))
        {
            PlayHitSound();
            ShowHitMessage("击中了固定靶! 加十分！");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ui.Score += 10;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("movingTarget"))
        {
            PlayHitSound();
            ShowHitMessage("击中了移动靶! 加二十分！");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ui.Score += 20;

            // 添加靶子反应效果
            TriggerTargetReaction(collision.gameObject);

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            PlayHitSound();
            ShowHitMessage("未射中！");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Destroy(gameObject);
        }
    }

    void TriggerTargetReaction(GameObject target)
    {
        // 停止动画播放
        Animator targetAnimator = target.GetComponent<Animator>();
        if (targetAnimator != null)
        {
            targetAnimator.enabled = false; // 停止动画
        }

        // 设置靶子为可下落状态
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb != null)
        {
            targetRb.isKinematic = false; // 解除刚体固定
            targetRb.useGravity = true;   // 启用重力
        }

        // 可选择性摧毁靶子
        Destroy(target, 2f); // 3 秒后销毁靶子
    }



    void ShowHitMessage(string message)
    {
        StartCoroutine(DisplayHitMessage(message));
    }

    void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
            Debug.Log("播放击中音乐");
        }
    }

    IEnumerator DisplayHitMessage(string message)
    {
        ui.SetHitMessage(message); 
        yield return new WaitForSeconds(hitDisplayTime);
        ui.ClearHitMessage(); 
    }
}
