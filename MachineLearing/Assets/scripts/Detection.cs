using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private Transform[] m_Points;
    [SerializeField] private float m_Range;
    [SerializeField] private LayerMask m_DetectionMask;

    public float[] Outputs { get; private set; }

    private void Update()
    {
        Detect();
    }

    private void Detect()
    {
        Outputs = new float[m_Points.Length];
        for (int i = 0; i < m_Points.Length; i++)
        {
            Vector3 _DirToPoint = m_Points[i].position - transform.position;
            _DirToPoint.Normalize();

            RaycastHit hit = default;

            if (Physics.Raycast(transform.position, _DirToPoint, out hit, m_Range, m_DetectionMask))
            {
                Debug.DrawLine(transform.position, transform.position + (_DirToPoint * m_Range), Color.blue);
                if (hit.collider != null)
                {
                    float _dis = Vector3.Distance(transform.position, hit.point);
                    float value = Mathf.Clamp(_dis, 0f, 1f);
                    Outputs[i] = value;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + (_DirToPoint * m_Range), Color.red);
            }
        }
    }
}