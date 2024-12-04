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
    public float hitDisplayTime = 2f; // ��ʾ��ʾʱ��
    public ArrowController archerControllerScript;

    public AudioClip hitSound; // ײ����Ч
    private AudioSource audioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ui = cam.GetComponent<UserGui>();
        archerControllerScript = GameObject.FindObjectOfType<ArrowController>();
        audioSource = GetComponent<AudioSource>(); // ��ȡ AudioSource ���
                                                   
        hitSound = Resources.Load<AudioClip>("Sound/hit"); // ȷ����Ч�ļ�·����ȷ
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
            ShowHitMessage("�����˹̶���! ��ʮ�֣�");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ui.Score += 10;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("movingTarget"))
        {
            PlayHitSound();
            ShowHitMessage("�������ƶ���! �Ӷ�ʮ�֣�");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ui.Score += 20;

            // ��Ӱ��ӷ�ӦЧ��
            TriggerTargetReaction(collision.gameObject);

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            PlayHitSound();
            ShowHitMessage("δ���У�");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Destroy(gameObject);
        }
    }

    void TriggerTargetReaction(GameObject target)
    {
        // ֹͣ��������
        Animator targetAnimator = target.GetComponent<Animator>();
        if (targetAnimator != null)
        {
            targetAnimator.enabled = false; // ֹͣ����
        }

        // ���ð���Ϊ������״̬
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb != null)
        {
            targetRb.isKinematic = false; // �������̶�
            targetRb.useGravity = true;   // ��������
        }

        // ��ѡ���Դݻٰ���
        Destroy(target, 2f); // 3 ������ٰ���
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
            Debug.Log("���Ż�������");
        }
    }

    IEnumerator DisplayHitMessage(string message)
    {
        ui.SetHitMessage(message); 
        yield return new WaitForSeconds(hitDisplayTime);
        ui.ClearHitMessage(); 
    }
}
