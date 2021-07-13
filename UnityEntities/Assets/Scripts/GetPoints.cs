using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPoints : MonoBehaviour
{
    [Header("QuadTree Settings")]
    [SerializeField] private int Size;

    [SerializeField] private int m_QuadCapicity;

    [SerializeField] private int m_AmountOfPoints;
    [SerializeField] private int m_MinSpeed, m_MaxSpeed;
    [SerializeField] private QuadTree<Point> m_Tree;
    [SerializeField] private float m_Range;
    private Camera m_Cam;
    private List<Point> m_Points;
    private Vector3 m_Mousepos;

    private List<Point> Points;

    private void Start()
    {
        m_Cam = Camera.main;
        m_Tree = new QuadTree<Point>(Size, m_QuadCapicity, Vector3.zero);
        SpawnPoints();
    }

    // Update is called once per frame
    private void Update()
    {
        SimulatePoints();
    }

    private void SpawnPoints()
    {
        Points = new List<Point>();                                 //Create Points
        int _amount = m_AmountOfPoints;                               //Caculate a random Amount
        for (int i = 0; i < _amount; i++)
        {
            float _x = Random.Range(-(float)m_Tree.Size, (float)m_Tree.Size);     //Caculate a random X
            float _y = Random.Range(-(float)m_Tree.Size, (float)m_Tree.Size);     //Caculate a random Y

            Point point = new Point
            {
                Position = new Vector3(_x, 0, _y),             //Set that worldPosition
                Speed = Random.Range(m_MinSpeed, m_MaxSpeed),
            };
            Points.Add(point);                                      //Add it to the total Points
        }
    }

    private void SimulatePoints()
    {
        GatherPoints();
        if (Points == null)
            return;

        for (int i = 0; i < Points.Count; i++)
        {
            Point _CurrentPoint = Points[i];
            float X = Random.Range(-1, 1) * _CurrentPoint.Speed * Time.deltaTime;
            float Y = Random.Range(-1, 1) * _CurrentPoint.Speed * Time.deltaTime;

            _CurrentPoint.Position = _CurrentPoint.Position + new Vector3(X, 0f, Y);
            Vector3 _newpos = _CurrentPoint.Position;
            if (_CurrentPoint.Position.x < m_Tree.position.x + -m_Tree.Size)
            {
                _newpos.x = m_Tree.position.x + m_Tree.Size;
                _CurrentPoint.Position = _newpos;
            }
            else if (_CurrentPoint.Position.x > m_Tree.position.x + m_Tree.Size)
            {
                _newpos.x = m_Tree.position.x + -m_Tree.Size;
                _CurrentPoint.Position = _newpos;
            }

            if (_CurrentPoint.Position.z < m_Tree.position.z + -m_Tree.Size)
            {
                _newpos.z = m_Tree.position.z + m_Tree.Size;
                _CurrentPoint.Position = _newpos;
            }
            else if (_CurrentPoint.Position.z > m_Tree.position.z + m_Tree.Size)
            {
                _newpos.z = m_Tree.position.z + -m_Tree.Size;
                _CurrentPoint.Position = _newpos;
            }

            Points[i] = _CurrentPoint;
        }

        m_Tree.SpawnPoints(Points);
    }

    private void GatherPoints()
    {
        Ray _ray = m_Cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out RaycastHit _hit))
        {
            m_Mousepos = _hit.point;
            m_Mousepos.y = 0;
            m_Points = m_Tree.GetPoints(_hit.point, m_Range);
        }
    }

    private void OnDrawGizmos()
    {
        if (m_Tree != null)
            m_Tree.OnDrawGizmos();
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(m_Mousepos, m_Range);
        Gizmos.DrawSphere(m_Mousepos, 1f);

        if (m_Points != null)
        {
            foreach (Point point in m_Points)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(point.Position, 0.7f);
            }
        }
    }
}