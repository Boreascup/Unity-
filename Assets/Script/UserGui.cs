using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGui : MonoBehaviour
{
    // Start is called before the first frame update
    public float Score = 0;
    public ArcherController archerControllerScript;
    private string hitMessage = "";
    public bool isCrosshairVisible = false; // 控制准星是否显示

    void Start()
    {
        archerControllerScript = GameObject.FindObjectOfType<ArcherController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 通过ArcherController的状态控制准星是否显示
        if (archerControllerScript != null)
        {
            isCrosshairVisible = archerControllerScript.isAiming;
        }
    }
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 14;
        style.normal.textColor = Color.black;

        // 定义游戏介绍文本内容
        string introText = "移动：WASD\n蓄力拉弓：空格\n发射弓箭：左键\n瞄准镜：右键\n鸟瞰图：F1";

        // 在屏幕左上角绘制游戏介绍文本
        GUI.Label(new Rect(10, 10, 300, 100), introText, style);
        style.fontSize = 24;
        style.normal.textColor = Color.red;
        // 在屏幕中上显示分数
        GUI.Label(new Rect(Screen.width / 2 - 15, 20, 150, 30), "分数: " + Score, style);
        // 显示射击次数
        style.fontSize = 18;
        GUI.Label(new Rect(Screen.width - 250, Screen.height - 30, 250, 30), "每个射击区域有十次射击机会 ", style);
        GUI.Label(new Rect(Screen.width - 180, Screen.height - 60, 150, 30), "山顶区剩余箭数: " + archerControllerScript.area1Shots, style);
        GUI.Label(new Rect(Screen.width - 180, Screen.height - 90, 150, 30), "迷宫区剩余箭数: " + archerControllerScript.area2Shots, style);

        GUI.Label(new Rect(50, Screen.height - 50, 150, 30), hitMessage, style);


        // 绘制准星
        if (isCrosshairVisible)
        {
            style.fontSize = 30;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;

            // 在屏幕中心显示 "+"
            GUI.Label(new Rect(Screen.width / 2 - 15, Screen.height / 2 - 15, 30, 30), "+", style);
        }

    }
    public void SetHitMessage(string message)
    {
        hitMessage = message;
    }

    public void ClearHitMessage()
    {
        hitMessage = "";
    }
}