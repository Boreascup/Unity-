using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    Animator animator;
    bool isPulling = false;
    float pullDuration = 0f;
    public float maxPullDuration = 2f; // �����ʱ��
    public float arrowSpeed = 1f; // ��ʸ�ٶ�
    float pullStrength;
    public Transform firePoint; // ��ʸ�����

    public Camera maincam; // �������
    public bool isAiming = false; // �Ƿ�������׼
    public float normalFOV = 60f; // ��ͨ�ӽ�
    public float aimFOV = 30f; // ��׼ʱ�ӽ�
    public float aimTransitionSpeed = 5f; // �ӽ�ƽ�������ٶ�

    public int area1Shots = 10;
    public int area2Shots = 10;

    public AudioClip arrowShootSound; // �����Ч
    private AudioSource audioSource;

    void Start()
    {
        // ��ȡ���ϵ�Animator���
        animator = GetComponent<Animator>();
        // ������Ч
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        // �Ҽ����£�������׼
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
        }
        // �Ҽ��ɿ���ȡ����׼
        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        // ƽ�������ӽ�
        if (maincam != null)
        {
            float targetFOV = isAiming ? aimFOV : normalFOV;
            maincam.fieldOfView = Mathf.Lerp(maincam.fieldOfView, targetFOV, aimTransitionSpeed * Time.deltaTime);
        }

        // ����Ƿ�����������ķ�Χ��
        if (IsInArea())
        {
            // ��ס�ո����������Fill ״̬��
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pullDuration = 0;
                isPulling = true;
                animator.SetBool("isPulling", true);
                animator.SetBool("isShooting", false);
                animator.SetBool("isHolding", false);
            }

            // ������ס�ո����������
            if (isPulling)
            {
                pullDuration += Time.deltaTime;
                pullStrength = Mathf.Clamp01(pullDuration / maxPullDuration);
                animator.SetFloat("pullStrength", pullStrength); // ����Animator�еĲ���
            }

            // �ɿ��ո�������ֵ�ǰ����״̬��Hold ״̬��
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isPulling = false;
                animator.SetBool("isPulling", false);
                animator.SetBool("isHolding", true);
            }

            // �����������������ʸ��Shoot ״̬��
            if (Input.GetMouseButtonDown(0) && animator.GetBool("isHolding"))
            {
                animator.SetBool("isShooting", true);
                animator.SetBool("isHolding", false);

                // �����¼�������ʸ����
            }
        }
    }


    /// <summary>
    /// �����ʸ���󶨵�Shoot�������¼�
    /// </summary>
    public void FireArrow()
    {
        // ����������
        Vector3 currentPosition = transform.position;
        Vector3 area1 = new Vector3(489f, 46f, 593f);
        Vector3 area2 = new Vector3(666f, 4f, 343f);
        float radius = 10f;

        bool inArea1 = Vector3.Distance(currentPosition, area1) <= radius;
        bool inArea2 = Vector3.Distance(currentPosition, area2) <= radius;

        if (inArea1 && area1Shots > 0)
        {
            area1Shots--; // ��������1����
            Debug.Log($"����1ʣ�������{area1Shots}");
        }
        else if (inArea2 && area2Shots > 0)
        {
            area2Shots--; // ��������2����
            Debug.Log($"����2ʣ�������{area2Shots}");
        }
        else
        {
            Debug.LogWarning("��ǰ�����޼������ã�");
            return;
        }

        // ������ʸ����
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

            // ���������Ч
            if (arrowShootSound != null)
            {
                audioSource.PlayOneShot(arrowShootSound);
                Debug.Log($"�����������Ч");
            }
        }


        // �����ص�Empty״̬
        animator.SetBool("isShooting", false);
    }


    /// <summary>
    /// ����Ƿ����������������
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
            Debug.Log("���������Χ�ڣ�");
            return false;
        }
    }
}
