using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGui : MonoBehaviour
{
    // Start is called before the first frame update
    public float Score = 0;
    public ArcherController archerControllerScript;
    private string hitMessage = "";
    public bool isCrosshairVisible = false; // ����׼���Ƿ���ʾ

    void Start()
    {
        archerControllerScript = GameObject.FindObjectOfType<ArcherController>();
    }

    // Update is called once per frame
    void Update()
    {
        // ͨ��ArcherController��״̬����׼���Ƿ���ʾ
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

        // ������Ϸ�����ı�����
        string introText = "�ƶ���WASD\n�����������ո�\n���乭�������\n��׼�����Ҽ�\n���ͼ��F1";

        // ����Ļ���Ͻǻ�����Ϸ�����ı�
        GUI.Label(new Rect(10, 10, 300, 100), introText, style);
        style.fontSize = 24;
        style.normal.textColor = Color.red;
        // ����Ļ������ʾ����
        GUI.Label(new Rect(Screen.width / 2 - 15, 20, 150, 30), "����: " + Score, style);
        // ��ʾ�������
        style.fontSize = 18;
        GUI.Label(new Rect(Screen.width - 250, Screen.height - 30, 250, 30), "ÿ�����������ʮ��������� ", style);
        GUI.Label(new Rect(Screen.width - 180, Screen.height - 60, 150, 30), "ɽ����ʣ�����: " + archerControllerScript.area1Shots, style);
        GUI.Label(new Rect(Screen.width - 180, Screen.height - 90, 150, 30), "�Թ���ʣ�����: " + archerControllerScript.area2Shots, style);

        GUI.Label(new Rect(50, Screen.height - 50, 150, 30), hitMessage, style);


        // ����׼��
        if (isCrosshairVisible)
        {
            style.fontSize = 30;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;

            // ����Ļ������ʾ "+"
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