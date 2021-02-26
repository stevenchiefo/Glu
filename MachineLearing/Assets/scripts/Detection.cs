using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private Transform m_LeftDetectionPos;
    [SerializeField] private Transform m_RightDetectionPos;
    [SerializeField] private Transform m_FrontDetectionPos;
    [SerializeField] private float m_Range;
    [SerializeField] private LayerMask m_DetectionMask;

    public int LeftDetectionRayOutput { get; private set; }
    public int RightDetectionRayOutput { get; private set; }
    public int FrontDetectionRayOutput { get; private set; }

    private void Update()
    {
        Detect();
    }

    private void Detect()
    {
        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.position + (m_FrontDetectionPos.position - transform.position).normalized * (m_Range * 2f), Color.red);
        Debug.DrawLine(transform.position, transform.position + (m_LeftDetectionPos.position - transform.position).normalized * m_Range, Color.red);
        Debug.DrawLine(transform.position, transform.position + (m_RightDetectionPos.position - transform.position).normalized * m_Range, Color.red);
        if (Physics.Raycast(transform.position, (m_LeftDetectionPos.position - transform.position).normalized, out hit, m_Range, m_DetectionMask))
        {
            if (hit.collider != null)
            {
                LeftDetectionRayOutput = 1;
                Debug.DrawLine(transform.position, hit.point, Color.blue);
            }
            else
            {
                LeftDetectionRayOutput = 0;
            }
        }
        if (Physics.Raycast(transform.position, (m_RightDetectionPos.position - transform.position).normalized, out hit, m_Range, m_DetectionMask))
        {
            if (hit.collider != null)
            {
                RightDetectionRayOutput = 1;
                Debug.DrawLine(transform.position, hit.point, Color.blue);
            }
            else
            {
                RightDetectionRayOutput = 0;
            }
        }
        if (Physics.Raycast(transform.position, (m_FrontDetectionPos.position - transform.position).normalized, out hit, m_Range * 1.5f, m_DetectionMask))
        {
            if (hit.collider != null)
            {
                FrontDetectionRayOutput = 1;
                Debug.DrawLine(transform.position, hit.point, Color.blue);
            }
            else
            {
                FrontDetectionRayOutput = 0;
            }
        }
    }
}