using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFollow : MonoBehaviour
{
    [SerializeField] private Vector3 m_Offset = new Vector3(0, 0, 3);
    private Camera m_Camera;

    private void Start()
    {
        m_Camera = Camera.main;
    }

    public void Update()
    {
        transform.position = m_Camera.transform.position + m_Offset;
        transform.eulerAngles = m_Camera.transform.eulerAngles;
    }
}