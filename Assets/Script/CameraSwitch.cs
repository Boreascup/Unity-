using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;  // 主摄像机
    public Camera topDownCamera;  // 鸟瞰图摄像机
    public RawImage topDownPreviewImage;  // 用于显示鸟瞰图的UI图像
    public RenderTexture renderTexture;  // 用于捕捉鸟瞰图的RenderTexture

    private bool isTopDownActive = false;  // 控制是否显示鸟瞰图

    void Start()
    {
        // 初始化时，隐藏鸟瞰图预览
        topDownPreviewImage.gameObject.SetActive(false);
    }

    void Update()
    {
        // 按下 F1 切换鸟瞰图
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleTopDownView();
        }
    }

    void ToggleTopDownView()
    {
        // 切换鸟瞰图的显示状态
        isTopDownActive = !isTopDownActive;

        if (isTopDownActive)
        {
            // 显示鸟瞰图摄像机，并把其渲染到小画面
            mainCamera.gameObject.SetActive(true);  // 主摄像机保持启用
            topDownCamera.gameObject.SetActive(true);  // 激活鸟瞰图摄像机

            // 将鸟瞰图渲染到小画面
            topDownPreviewImage.gameObject.SetActive(true);
            topDownPreviewImage.texture = renderTexture;  // 设置RenderTexture为RawImage的纹理
        }
        else
        {
            // 关闭鸟瞰图摄像机，隐藏鸟瞰图
            topDownCamera.gameObject.SetActive(false);  // 关闭鸟瞰图摄像机

            // 隐藏鸟瞰图预览
            topDownPreviewImage.gameObject.SetActive(false);
        }
    }
}
