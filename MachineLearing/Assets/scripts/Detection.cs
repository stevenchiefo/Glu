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
        Debug.DrawRay(transform.position,(m_FrontDetectionPos.position - transform.position);
        if(Physics.Raycast(transform.position, m_LeftDetectionPos.position - transform.position, out hit,m_Range, m_DetectionMask))
        {
            if(hit.collider != null)
            {
                LeftDetectionRayOutput = 1;
            }
            else
            {
                LeftDetectionRayOutput = 0;
            }
        }
        if (Physics.Raycast(transform.position, m_RightDetectionPos.position - transform.position, out hit, m_Range, m_DetectionMask))
        {
            if (hit.collider != null)
            {
                RightDetectionRayOutput = 1;
            }
            else
            {
                RightDetectionRayOutput = 0;
            }
        }
        if (Physics.Raycast(transform.position, m_FrontDetectionPos.position - transform.position, out hit, m_Range, m_DetectionMask))
        {
            if (hit.collider != null)
            {
                FrontDetectionRayOutput = 1;
            }
            else
            {
                FrontDetectionRayOutput = 0;
            }
        }
    }
}
