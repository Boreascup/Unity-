
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    // ���
    public Transform tourCamera;

    #region ����ƶ�����
    public float moveSpeed = 10.0f;
    public float rotateSpeed = 250.0f;
    public float shiftRate = 2.0f; // ��סShift����
    public float minDistance = 5f; // ����벻�ɴ����ı������С���루С�ڵ���0ʱ�ɴ�͸�κα��棩
    #endregion

    #region �˶��ٶȺ���ÿ��������ٶȷ���
    private Vector3 direction = Vector3.zero;
    private Vector3 speedForward;
    private Vector3 speedBack;
    private Vector3 speedLeft;
    private Vector3 speedRight;

    #endregion

    private float verticalRotation = 0.0f; // ��ֱ����ĽǶ�

    void Start()
    {
        if (tourCamera == null) tourCamera = gameObject.transform;
    }

    void Update()
    {
        GetDirection();

        // ����Ƿ��벻�ɴ�͸�������
        RaycastHit hit;
        while (Physics.Raycast(tourCamera.position, direction, out hit, minDistance))
        {
            // ��ȥ��ֱ�ڲ��ɴ�͸������˶��ٶȷ���
            float angle = Vector3.Angle(direction, hit.normal);
            float magnitude = Vector3.Magnitude(direction) * Mathf.Cos(Mathf.Deg2Rad * (180 - angle));
            direction += hit.normal * magnitude;
        }

        // ��������Ĵ�ֱ�߶�
        //tourCamera.localPosition = new Vector3(tourCamera.localPosition.x, 1f, tourCamera.localPosition.z);

        // �ƶ����
        tourCamera.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    private void GetDirection()
    {
        #region �����ƶ�
        if (Input.GetKeyDown(KeyCode.LeftShift)) moveSpeed *= shiftRate;
        if (Input.GetKeyUp(KeyCode.LeftShift)) moveSpeed /= shiftRate;
        #endregion

        #region �����ƶ�
        // ��λ
        speedForward = Vector3.zero;
        speedBack = Vector3.zero;
        speedLeft = Vector3.zero;
        speedRight = Vector3.zero;

        // ��ȡ�����ˮƽ�����Ƴ���ֱ������
        Vector3 cameraForward = tourCamera.forward;
        cameraForward.y = 0; // �Ƴ���ֱ����
        cameraForward.Normalize(); // ���¹�һ��

        Vector3 cameraRight = tourCamera.right;
        cameraRight.y = 0; // �Ƴ���ֱ����
        cameraRight.Normalize(); // ���¹�һ��

        // ��ȡ��������
        if (Input.GetKey(KeyCode.W)) speedForward = cameraForward;
        if (Input.GetKey(KeyCode.S)) speedBack = -cameraForward;
        if (Input.GetKey(KeyCode.A)) speedLeft = -cameraRight;
        if (Input.GetKey(KeyCode.D)) speedRight = cameraRight;

        // �ϲ�����
        direction = speedForward + speedBack + speedLeft + speedRight;
        #endregion

        #region �����ת
        // ��ȡ�������
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

        // ˮƽ������ת�����ң�
        tourCamera.RotateAround(tourCamera.position, Vector3.up, mouseX);

        // ��ֱ������ת�����£��������� ��90 ��
        verticalRotation -= mouseY; // ����ƶ���������ת�����෴
        verticalRotation = Mathf.Clamp(verticalRotation, -45f, 45f);

        // Ӧ�ô�ֱ��ת
        tourCamera.localEulerAngles = new Vector3(verticalRotation, tourCamera.localEulerAngles.y, 0);
        #endregion
    }


    /// <summary>
    /// ����һ��Vector3����ת������תָ���ǶȺ����õ���������
    /// </summary>
    /// <param name="source">��תǰ��ԴVector3</param>
    /// <param name="axis">��ת��</param>
    /// <param name="angle">��ת�Ƕ�</param>
    /// <returns>��ת��õ�����Vector3</returns>
    public Vector3 V3RotateAround(Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis); // ��תϵ��
        return q * source; // ����Ŀ���
    }
}
