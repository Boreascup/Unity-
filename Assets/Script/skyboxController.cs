using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    public Material[] skyboxes; // ��պв�������
    private int currentSkyboxIndex = 0; // ��ǰ��պ�����
    public float switchInterval = 5f; // �л����ʱ��

    private void Start()
    {
        if (skyboxes.Length == 0)
        {
            Debug.LogError("����Inspector�з�����պв��ʣ�");
            return;
        }
        RenderSettings.skybox = skyboxes[currentSkyboxIndex]; // ���ó�ʼ��պ�
        InvokeRepeating("SwitchSkybox", switchInterval, switchInterval); // ��ʱ�����л�����
    }

    private void SwitchSkybox()
    {
        currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxes.Length; // ѭ���л�����
        RenderSettings.skybox = skyboxes[currentSkyboxIndex]; // Ӧ������պ�
        Debug.Log($"�л�����պ�: {skyboxes[currentSkyboxIndex].name}");
    }
}

