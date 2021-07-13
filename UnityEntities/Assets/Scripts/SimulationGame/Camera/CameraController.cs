using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float m_Speed;

    [Header("Keys")]
    [SerializeField] private KeyCode m_Forward;

    [SerializeField] private KeyCode m_Backward;
    [SerializeField] private KeyCode m_Left;
    [SerializeField] private KeyCode m_Right;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 _dir = GetInput();
        transform.position += _dir * m_Speed * Time.deltaTime;
    }

    private Vector3 GetInput()
    {
        int _z = 0;
        int _x = 0;

        if (Input.GetKey(m_Forward))
            _z += 1;
        if (Input.GetKey(m_Backward))
            _z -= 1;
        if (Input.GetKey(m_Left))
            _x -= 1;
        if (Input.GetKey(m_Right))
            _x += 1;

        return new Vector3(_x, 0f, _z);
    }
}