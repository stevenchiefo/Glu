using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree : MonoBehaviour
{
    [SerializeField] private int Size;
    [SerializeField] private int m_MaxQuadCapicity;
    private Quad MainQuad;
    private List<Point> Points;

    private void Start()
    {

    }

    private void Update()
    {
        SpawnPoints();
    }

    private bool CheckInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void SpawnPoints()
    {
        if (CheckInput())
        {
            MainQuad = new Quad
            {
                Size = Size,
                CenterPosition = transform.position,
                ChildQuads = new List<Quad>(),
                Points = new List<Point>(),
                Capicity = m_MaxQuadCapicity,
            };

            Points = new List<Point>();
            int _amount = Random.Range(0, 1000);
            for (int i = 0; i < _amount; i++)
            {
                float _x = Random.Range(-(float)Size / 2f, (float)Size / 2f);
                float _y = Random.Range(-(float)Size / 2f, (float)Size / 2f);

                Point point = new Point
                {
                    WorldPosition = new Vector3(_x, 0, _y),
                };
                Points.Add(point);
            }
            CheckPoints();
        }
    }

    private void CheckPoints()
    {
        foreach (Point i in Points)
        {
            CheckWhereIn(i);
        }
    }
    public void CheckWhereIn(Point point)
    {
        MainQuad.AddPoint(point);
    }

    private void OnDrawGizmos()
    {
        List<Quad> quads = new List<Quad>();
        quads.Add(MainQuad);
        for (int i = 0; i < quads.Count; i++)
        {
            List<Quad> ChildQuads = quads[i].GetChilderen();
            foreach (Quad item in ChildQuads)
            {
                quads.Add(item);
            }
        }


        foreach (Quad quad in quads)
        {
            Vector3[] _points = new Vector3[]
            {
                new Vector3(quad.Size/2,0,quad.Size/2),
                new Vector3(quad.Size/2,0,-quad.Size/2),
                new Vector3(-quad.Size/2,0,-quad.Size/2),
                new Vector3(-quad.Size/2,0,quad.Size/2),
            };
            for (int i = 0; i < _points.Length; i++)
            {
                Gizmos.color = Color.blue;
                Vector3 pos_1 = Vector3.zero;
                Vector3 pos_2 = Vector3.zero;
                if (i == _points.Length - 1)
                {
                    pos_1 = _points[i] + quad.CenterPosition;
                    pos_2 = _points[0] + quad.CenterPosition;
                }
                else
                {
                    pos_1 = _points[i] + quad.CenterPosition;
                    pos_2 = _points[i + 1] + quad.CenterPosition;
                }


                Gizmos.DrawLine(pos_1, pos_2);
            }
            Vector3 _size = new Vector3(0.05f, 0.05f, 0.05f);

            Gizmos.DrawCube(quad.CenterPosition, _size);

        }

        if (Points != null)
        {
            foreach (Point point in Points)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(point.WorldPosition, 0.1f);
            }
        }
    }
}

public struct Quad
{
    public Vector3 CenterPosition;
    public float Size;
    public List<Point> Points;
    public List<Quad> ChildQuads;
    public int Capicity;
    public Quad(Vector3 _pos, float _size, int _cap)
    {
        CenterPosition = _pos;
        Size = _size;
        Points = new List<Point>();
        ChildQuads = new List<Quad>();
        Capicity = _cap;
    }


    public void AddPoint(Point _point)
    {
        if (ChildQuads.Count == 0)
        {
            Points.Add(_point);
            if (Points.Count > Capicity)
            {
                DevidePoints();
            }
        }
        else
        {
            for (int i = 0; i < ChildQuads.Count; i++)
            {
                if (ChildQuads[i].IsWithinBounds(_point.WorldPosition))
                {
                    ChildQuads[i].AddPoint(_point);
                    break;
                }
            }
        }
    }

    public void RemovePoint(Point _point)
    {
        Points.Remove(_point);
    }

    public bool IsWithinBounds(Vector3 _point)
    {
        float X = CenterPosition.x + Size;
        float Y = CenterPosition.y + Size;
        if (_point.x > -X && _point.x < X && _point.y > -Y && _point.y < Y)
        {
            return true;
        }
        return false;
    }

    private void DevidePoints()
    {
        Quad[] newquads = new Quad[4];
        float _size = Size / 4f;

        Vector3[] _Offsets = new Vector3[]
        {
            new Vector3(_size, 0, _size),
            new Vector3(_size, 0, -_size),
            new Vector3(-_size, 0, -_size),
            new Vector3(-_size, 0, _size),
        };

        for (int i = 0; i < newquads.Length; i++)
        {
            Quad _currentQuad = new Quad(CenterPosition + _Offsets[i], Size / 2f, Capicity);
            for (int p = 0; p < Points.Count; p++)
            {
                if (_currentQuad.IsWithinBounds(Points[p].WorldPosition))
                {
                    _currentQuad.AddPoint(Points[p]);
                    RemovePoint(Points[p]);
                }
            }
            newquads[i] = _currentQuad;
        }
        foreach (Quad item in newquads)
        {
            ChildQuads.Add(item);
        }
    }

    public List<Quad> GetChilderen()
    {
        return ChildQuads;
    }


}

public struct Point
{
    public Vector3 WorldPosition;
}
