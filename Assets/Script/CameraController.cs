
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    // 相机
    public Transform tourCamera;

    #region 相机移动参数
    public float moveSpeed = 10.0f;
    public float rotateSpeed = 250.0f;
    public float shiftRate = 2.0f; // 按住Shift加速
    public float minDistance = 5f; // 相机离不可穿过的表面的最小距离（小于等于0时可穿透任何表面）
    #endregion

    #region 运动速度和其每个方向的速度分量
    private Vector3 direction = Vector3.zero;
    private Vector3 speedForward;
    private Vector3 speedBack;
    private Vector3 speedLeft;
    private Vector3 speedRight;

    #endregion

    private float verticalRotation = 0.0f; // 垂直方向的角度

    void Start()
    {
        if (tourCamera == null) tourCamera = gameObject.transform;
    }

    void Update()
    {
        GetDirection();

        // 检测是否离不可穿透表面过近
        RaycastHit hit;
        while (Physics.Raycast(tourCamera.position, direction, out hit, minDistance))
        {
            // 消去垂直于不可穿透表面的运动速度分量
            float angle = Vector3.Angle(direction, hit.normal);
            float magnitude = Vector3.Magnitude(direction) * Mathf.Cos(Mathf.Deg2Rad * (180 - angle));
            direction += hit.normal * magnitude;
        }

        // 限制相机的垂直高度
        //tourCamera.localPosition = new Vector3(tourCamera.localPosition.x, 1f, tourCamera.localPosition.z);

        // 移动相机
        tourCamera.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    private void GetDirection()
    {
        #region 加速移动
        if (Input.GetKeyDown(KeyCode.LeftShift)) moveSpeed *= shiftRate;
        if (Input.GetKeyUp(KeyCode.LeftShift)) moveSpeed /= shiftRate;
        #endregion

        #region 键盘移动
        // 复位
        speedForward = Vector3.zero;
        speedBack = Vector3.zero;
        speedLeft = Vector3.zero;
        speedRight = Vector3.zero;

        // 获取相机的水平方向（移除垂直分量）
        Vector3 cameraForward = tourCamera.forward;
        cameraForward.y = 0; // 移除垂直分量
        cameraForward.Normalize(); // 重新归一化

        Vector3 cameraRight = tourCamera.right;
        cameraRight.y = 0; // 移除垂直分量
        cameraRight.Normalize(); // 重新归一化

        // 获取按键输入
        if (Input.GetKey(KeyCode.W)) speedForward = cameraForward;
        if (Input.GetKey(KeyCode.S)) speedBack = -cameraForward;
        if (Input.GetKey(KeyCode.A)) speedLeft = -cameraRight;
        if (Input.GetKey(KeyCode.D)) speedRight = cameraRight;

        // 合并方向
        direction = speedForward + speedBack + speedLeft + speedRight;
        #endregion

        #region 鼠标旋转
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

        // 水平方向旋转（左右）
        tourCamera.RotateAround(tourCamera.position, Vector3.up, mouseX);

        // 垂直方向旋转（上下），限制在 ±90 度
        verticalRotation -= mouseY; // 鼠标移动方向与旋转方向相反
        verticalRotation = Mathf.Clamp(verticalRotation, -45f, 45f);

        // 应用垂直旋转
        tourCamera.localEulerAngles = new Vector3(verticalRotation, tourCamera.localEulerAngles.y, 0);
        #endregion
    }


    /// <summary>
    /// 计算一个Vector3绕旋转中心旋转指定角度后所得到的向量。
    /// </summary>
    /// <param name="source">旋转前的源Vector3</param>
    /// <param name="axis">旋转轴</param>
    /// <param name="angle">旋转角度</param>
    /// <returns>旋转后得到的新Vector3</returns>
    public Vector3 V3RotateAround(Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis); // 旋转系数
        return q * source; // 返回目标点
    }
}
