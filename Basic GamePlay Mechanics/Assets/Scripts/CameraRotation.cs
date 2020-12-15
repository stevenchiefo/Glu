using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float m_RotationSpeed;
    private Camera m_Camera;

    private void Start()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float Yrot = Input.GetAxis("Mouse X");
        Vector3 angels = transform.eulerAngles;
        angels.y += Yrot * m_RotationSpeed * Time.deltaTime;
        transform.eulerAngles = angels;
    }

    public Vector3 GetDirectionCameraIsFacing()
    {
        Vector3 dir = transform.position - m_Camera.transform.position;
        dir.y = 0;
        return dir;
    }
}