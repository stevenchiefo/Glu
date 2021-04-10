using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    public float Size { get; private set; }             //Size for main quad
    public Vector3 position { get; private set; }       //World Position Of QuadTree

    public int GridSize { get; private set; }           //Total grid Size
    public int MaxQuadCapicity { get; private set; }    //Capicity per quad
    private Quad MainQuad;                              //Main/Start Quad
    private List<Point> Points;                         //All Points in world

    public QuadTree(int TotalSize, int MaxCapicity, Vector3 _pos)
    {
        GridSize = TotalSize;
        MaxQuadCapicity = MaxCapicity;
        position = _pos;

        Size = GridSize / 2f;                           //caculate the correct size for the mainQuad
    }

    public void SpawnPoints(List<Point> _Points)
    {
        MainQuad = new Quad(position, Size, 10);          //Create MainQuad
        Points = _Points;
        CheckPoints();
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

    public List<Point> GetPoints(Vector3 pos, float _range)
    {
        return MainQuad.GetPoints(pos, _range);
    }

    public void OnDrawGizmos()
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

        //List<Point> _LocalDrawingQuadPoints = MainQuad.GetListPoints();

        //if (_LocalDrawingQuadPoints != null)
        //{
        //    foreach (Point point in _LocalDrawingQuadPoints)
        //    {
        //        Gizmos.color = Color.black;
        //        Gizmos.DrawSphere(point.WorldPosition, 2f);
        //    }
        //}

        //if (Points != null)
        //{
        //    foreach (Point point in Points)
        //    {
        //        Gizmos.color = Color.green;
        //        Gizmos.DrawSphere(point.WorldPosition, 1f);
        //    }
        //}
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

    public bool ExpandSearch;

    public Quad(Vector3 _pos, float _size, int _cap)
    {
        CenterPosition = _pos;
        Size = _size;
        Points = new List<Point>();
        ChildQuads = new List<Quad>();
        Capicity = _cap;
        BoundsMin = new Vector3(-Size, 0, -Size) + CenterPosition;
        BoundsMax = new Vector3(Size, 0, Size) + CenterPosition;
        ExpandSearch = false;
    }

    public List<Point> GetPoints(Vector3 _pos, float _Range)
    {
        if (ChildQuads == null)
        {
            return Points;
        }

        if (OverLap(_pos, _Range))
            ExpandSearch = true;

        List<Point> _points = new List<Point>();
        if (ChildQuads.Count == 0)
        {
            foreach (Point item in Points)
            {
                float _distance = Vector3.Distance(_pos, item.WorldPosition);
                if (_distance <= _Range)
                {
                    _points.Add(item);
                }
            }
            return _points;
        }
        else
        {
            if (ChildQuads != null)
            {
                bool expand = ExpandSearch;
                foreach (Quad _quad in ChildQuads)
                {
                    if (_quad.IsWithinBounds(_pos) || expand)
                    {
                        List<Point> _localPoints = _quad.GetPoints(_pos, _Range);
                        foreach (Point LocalP in _localPoints)
                        {
                            _points.Add(LocalP);
                        }
                    }

                    if (_quad.ExpandSearch)
                    {
                        expand = true;

                        ExpandSearch = true;
                    }
                }
            }
            return _points;
        }
    }

    private bool OverLap(Vector3 _pos, float _Range)
    {
        Vector3[] _Position = new Vector3[]
        {
            new Vector3(_Range,0,0),
            new Vector3(_Range,0,_Range),
            new Vector3(0,0,_Range),
            new Vector3(-_Range,0,_Range),
            new Vector3(-_Range,0,0),
            new Vector3(-_Range,0,-_Range),
            new Vector3(0,0,-_Range),
            new Vector3(_Range,0,-_Range),
        };

        foreach (Vector3 _offset in _Position)
        {
            if (IsWithinBounds(_pos + _offset) == false)
            {
                return true;
            }
        }
        return false;
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
        if (BoundsMax.x >= _point.x && BoundsMin.x <= _point.x)
        {
            if (BoundsMax.z >= _point.z && BoundsMin.z <= _point.z)
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
            List<Point> _HaveTOBeRemoved = new List<Point>();

            for (int p = 0; p < Points.Count; p++)
            {
                if (_currentQuad.IsWithinBounds(Points[p].WorldPosition))
                {
                    _currentQuad.AddPoint(Points[p]);
                    _HaveTOBeRemoved.Add(Points[p]);
                }
            }

            newquads[i] = _currentQuad;
        }
        Points.Clear();

        foreach (Quad item in newquads)
        {
            ChildQuads.Add(item);
        }
    }

    public List<Quad> GetChilderen()
    {
        return ChildQuads;
    }

    public List<Point> GetListPoints()
    {
        if (ChildQuads == null)
            return Points;

        if (ChildQuads.Count == 0)
        {
            return Points;
        }
        else
        {
            List<Point> _Points = new List<Point>();
            foreach (Quad child in ChildQuads)
            {
                List<Point> _localPoints = child.GetListPoints();
                foreach (Point _p in _localPoints)
                {
                    _Points.Add(_p);
                }
            }
            return _Points;
        }
    }
}

public struct Point
{
    public Vector3 WorldPosition;
    public float Speed;
}