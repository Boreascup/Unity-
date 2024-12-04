using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;  // �������
    public Camera topDownCamera;  // ���ͼ�����
    public RawImage topDownPreviewImage;  // ������ʾ���ͼ��UIͼ��
    public RenderTexture renderTexture;  // ���ڲ�׽���ͼ��RenderTexture

    private bool isTopDownActive = false;  // �����Ƿ���ʾ���ͼ

    void Start()
    {
        // ��ʼ��ʱ���������ͼԤ��
        topDownPreviewImage.gameObject.SetActive(false);
    }

    void Update()
    {
        // ���� F1 �л����ͼ
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleTopDownView();
        }
    }

    void ToggleTopDownView()
    {
        // �л����ͼ����ʾ״̬
        isTopDownActive = !isTopDownActive;

        if (isTopDownActive)
        {
            // ��ʾ���ͼ���������������Ⱦ��С����
            mainCamera.gameObject.SetActive(true);  // ���������������
            topDownCamera.gameObject.SetActive(true);  // �������ͼ�����

            // �����ͼ��Ⱦ��С����
            topDownPreviewImage.gameObject.SetActive(true);
            topDownPreviewImage.texture = renderTexture;  // ����RenderTextureΪRawImage������
        }
        else
        {
            // �ر����ͼ��������������ͼ
            topDownCamera.gameObject.SetActive(false);  // �ر����ͼ�����

            // �������ͼԤ��
            topDownPreviewImage.gameObject.SetActive(false);
        }
    }
}
