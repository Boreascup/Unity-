using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // 目标物体
    public Vector3 offset;  // 摄像机相对目标的偏移

    void Start()
    {
        // 设置初始偏移
        offset = new Vector3(0, 50, 0);  // 高度50单位
    }

    void Update()
    {
        // 设置摄像机位置，始终在目标物体的上方，并保持固定的高度
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);  // 可以选择使摄像机始终朝向目标
        }
    }
}



