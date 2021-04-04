using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPoints : MonoBehaviour
{
    [SerializeField] private QuadTree m_Tree;
    [SerializeField] private float Range;
    private Camera m_Cam;
    private List<Point> m_Points;
    private Vector3 m_Mousepos;

    private void Start()
    {
        m_Cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        GatherPoints();
    }

    private void GatherPoints()
    {
        Ray _ray = m_Cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out RaycastHit _hit))
        {
            m_Mousepos = _hit.point;
            m_Mousepos.y = 0;
            m_Points = m_Tree.GetPoints(_hit.point, Range);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(m_Mousepos, Range);

        if (m_Points != null)
        {
            foreach (Point point in m_Points)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(point.WorldPosition, 2f);
            }
        }
    }
}