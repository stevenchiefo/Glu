using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 m_Offset;
    private GameObject m_Player;

    private void Start()
    {
        m_Player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = m_Player.transform.position + m_Offset;
    }
}