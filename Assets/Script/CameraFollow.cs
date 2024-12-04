using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Ŀ������
    public Vector3 offset;  // ��������Ŀ���ƫ��

    void Start()
    {
        // ���ó�ʼƫ��
        offset = new Vector3(0, 50, 0);  // �߶�50��λ
    }

    void Update()
    {
        // ���������λ�ã�ʼ����Ŀ��������Ϸ��������̶ֹ��ĸ߶�
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);  // ����ѡ��ʹ�����ʼ�ճ���Ŀ��
        }
    }
}



