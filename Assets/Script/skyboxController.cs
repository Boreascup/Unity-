using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    public Material[] skyboxes; // 天空盒材质数组
    private int currentSkyboxIndex = 0; // 当前天空盒索引
    public float switchInterval = 5f; // 切换间隔时间

    private void Start()
    {
        if (skyboxes.Length == 0)
        {
            Debug.LogError("请在Inspector中分配天空盒材质！");
            return;
        }
        RenderSettings.skybox = skyboxes[currentSkyboxIndex]; // 设置初始天空盒
        InvokeRepeating("SwitchSkybox", switchInterval, switchInterval); // 定时调用切换方法
    }

    private void SwitchSkybox()
    {
        currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxes.Length; // 循环切换索引
        RenderSettings.skybox = skyboxes[currentSkyboxIndex]; // 应用新天空盒
        Debug.Log($"切换到天空盒: {skyboxes[currentSkyboxIndex].name}");
    }
}

