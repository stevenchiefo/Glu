using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPoints : MonoBehaviour
{
    [Header("QuadTree Settings")]
    [SerializeField] private int Size;
    [SerializeField] private int QuadCapicity;

    [SerializeField] private int AmountOfPoints;
    [SerializeField] private QuadTree m_Tree;
    [SerializeField] private float Range;
    private Camera m_Cam;
    private List<Point> m_Points;
    private Vector3 m_Mousepos;

    private List<Point> Points;

    private void Start()
    {
        m_Cam = Camera.main;
        m_Tree = new QuadTree(Size, QuadCapicity, Vector3.zero);
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
        int _amount = AmountOfPoints;                               //Caculate a random Amount
        for (int i = 0; i < _amount; i++)
        {
            float _x = Random.Range(-(float)m_Tree.Size, (float)m_Tree.Size);     //Caculate a random X
            float _y = Random.Range(-(float)m_Tree.Size, (float)m_Tree.Size);     //Caculate a random Y

            Point point = new Point
            {
                WorldPosition = new Vector3(_x, 0, _y),             //Set that worldPosition
                Speed = Random.Range(5, 10),
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


            _CurrentPoint.WorldPosition = _CurrentPoint.WorldPosition + new Vector3(X, 0f, Y);

            if (_CurrentPoint.WorldPosition.x < m_Tree.position.x + -m_Tree.Size)
            {
                _CurrentPoint.WorldPosition.x = m_Tree.position.x + m_Tree.Size;
            }
            else if (_CurrentPoint.WorldPosition.x > m_Tree.position.x + m_Tree.Size)
            {
                _CurrentPoint.WorldPosition.x = m_Tree.position.x + -m_Tree.Size;
            }

            if (_CurrentPoint.WorldPosition.z < m_Tree.position.z + -m_Tree.Size)
            {
                _CurrentPoint.WorldPosition.z = m_Tree.position.z + m_Tree.Size;
            }
            else if (_CurrentPoint.WorldPosition.z > m_Tree.position.z + m_Tree.Size)
            {
                _CurrentPoint.WorldPosition.z = m_Tree.position.z + -m_Tree.Size;
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
            m_Points = m_Tree.GetPoints(_hit.point, Range);
        }
    }

    private void OnDrawGizmos()
    {
        if (m_Tree != null)
            m_Tree.OnDrawGizmos();
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