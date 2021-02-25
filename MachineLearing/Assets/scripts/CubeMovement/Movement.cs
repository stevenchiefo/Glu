using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rb;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_MaxSpeed;

    public void MoveTo(float _rotation)
    {
        Vector3 _rot = transform.eulerAngles;
        _rot.y += _rotation * Time.deltaTime;
        transform.eulerAngles = _rot;
        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
    }
}
