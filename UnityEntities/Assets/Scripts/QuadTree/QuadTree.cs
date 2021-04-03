using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree : MonoBehaviour
{
    [SerializeField] private int GridSize;              //Total grid Size
    [SerializeField] private int m_MaxQuadCapicity;     //Capicity per quad
    private float Size;                                 //Size for main quad
    private Quad MainQuad;                              //Main/Start Quad
    private List<Point> Points;                         //All Points in world

    private void Start()
    {
        Size = GridSize / 2f;                           //caculate the correct size for the mainQuad
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
            MainQuad = new Quad(transform.position, Size, 10);          //Create MainQuad


            Points = new List<Point>();                                 //Create Points
            int _amount = Random.Range(0, 1000);                        //Caculate a random Amount 
            for (int i = 0; i < _amount; i++)
            {
                float _x = Random.Range(-(float)Size, (float)Size);     //Caculate a random X
                float _y = Random.Range(-(float)Size, (float)Size);     //Caculate a random Y

                Point point = new Point
                {
                    WorldPosition = new Vector3(_x, 0, _y),             //Set that worldPosition
                };
                Points.Add(point);                                      //Add it to the total Points
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
            if (ChildQuads != null)
            {
                foreach (Quad item in ChildQuads)
                {
                    quads.Add(item);
                }
            }
        }


        foreach (Quad quad in quads)
        {
            Vector3[] _points = new Vector3[]
            {
                new Vector3(quad.Size,0,quad.Size),
                new Vector3(quad.Size,0,-quad.Size),
                new Vector3(-quad.Size,0,-quad.Size),
                new Vector3(-quad.Size,0,quad.Size),
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
            Vector3 _size = new Vector3(0.1f, 0.1f, 0.1f);

            Gizmos.DrawCube(quad.CenterPosition, _size);

        }

        if (Points != null)
        {
            foreach (Point point in Points)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(point.WorldPosition, 1f);
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

    public Vector3 BoundsMin;
    public Vector3 BoundsMax;

    public Quad(Vector3 _pos, float _size, int _cap)
    {
        CenterPosition = _pos;
        Size = _size;
        Points = new List<Point>();
        ChildQuads = new List<Quad>();
        Capicity = _cap;
        BoundsMin = new Vector3(-Size, 0, -Size) + CenterPosition;
        BoundsMax = new Vector3(Size, 0, Size) + CenterPosition;
    }

    public List<Point> GetPoints(Vector3 _pos, float _Range)
    {
        List<Point> _points = new List<Point>();
        if(ChildQuads.Count == 0)
        {
            
            foreach (Point item in Points)
            {
                float _distance = Vector3.Distance(_pos, item.WorldPosition);
                if(_distance <= _Range)
                {
                    _points.Add(item);
                }
            }
            return _points;
        }
        else
        {
            foreach (Quad _quad in ChildQuads)
            {
                if (_quad.IsWithinBounds(_pos))
                {
                    _points = _quad.GetPoints(_pos, _Range);
                }
            }
            return _points;
        }
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
        if (BoundsMax.x > _point.x && BoundsMin.x < _point.x)
        {
            if(BoundsMax.z > _point.z && BoundsMin.z < _point.z)
            return true;
        }
        return false;
    }

    private void DevidePoints()
    {
        Quad[] newquads = new Quad[4];
        float _size = Size / 2f;

        Vector3[] _Offsets = new Vector3[]
        {
            new Vector3(_size, 0, _size),
            new Vector3(_size, 0, -_size),
            new Vector3(-_size, 0, -_size),
            new Vector3(-_size, 0, _size),
        };

        for (int i = 0; i < newquads.Length; i++)
        {
            Quad _currentQuad = new Quad(CenterPosition + _Offsets[i], _size, Capicity);
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
