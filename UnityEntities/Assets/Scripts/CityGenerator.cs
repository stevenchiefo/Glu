using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int SizeX;

    [SerializeField] private int SizeY;
    [SerializeField] private float m_NodeSize;
    [SerializeField] private Vector2 m_Offset;
    [SerializeField] private GameObject m_HousePrefab;
    [SerializeField] private GameObject m_StreetPrefab;
    [SerializeField] private List<GameObject> Buildings;
    private CityNode[,] m_GridNodes;

    private void Start()
    {
        CreateGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearGrid();
            ClearBuildings();
            GenerateCity();
            PlacePrefabsOnGrid();
        }
    }

    private void CreateGrid()
    {
        float size = m_NodeSize;
        m_GridNodes = new CityNode[SizeX, SizeY];
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                m_GridNodes[x, y] = new CityNode
                {
                    IndexX = x,
                    IndexY = y,
                    WorldPos = new Vector2(x * size, y * size) + new Vector2(transform.position.x, transform.position.z),
                    Type = CityTileType.none,
                    FollewedStreets = 0,
                };
            }
        }
    }

    private void GenerateCity()
    {
        Vector2Int _pointToFirstStreetTile = CaculateFirstStreetPoint();
        m_GridNodes[_pointToFirstStreetTile.x, _pointToFirstStreetTile.y].Type = CityTileType.Street;

        List<Vector2Int> OpenList = new List<Vector2Int>();
        List<Vector2Int> ClosedList = new List<Vector2Int>();

        OpenList.Add(_pointToFirstStreetTile);
        int looped = 0;
        while (OpenList.Count > 0 && looped < 10000)
        {
            int i = 0;

            Vector2Int[] _Neigbours = GetNeighBourPointers(OpenList[i]);
            foreach (Vector2Int _neighbour in _Neigbours)
            {
                if (m_GridNodes[_neighbour.x, _neighbour.y].Type != CityTileType.none)
                    continue;

                CityTileType _type = CaculateTile(m_GridNodes[OpenList[i].x, OpenList[i].y].FollewedStreets);
                if (_type == CityTileType.Street)
                {
                    m_GridNodes[_neighbour.x, _neighbour.y].FollewedStreets = m_GridNodes[OpenList[i].x, OpenList[i].y].FollewedStreets + 1;
                }
                m_GridNodes[_neighbour.x, _neighbour.y].Type = _type;
                if (!ClosedList.Contains(_neighbour) && _type != CityTileType.StreetEnd && _type != CityTileType.House)
                {
                    OpenList.Add(_neighbour);
                }
                else
                {
                    if (_type == CityTileType.StreetEnd || _type == CityTileType.House)
                    {
                        ClosedList.Add(_neighbour);
                    }
                }
            }
            ClosedList.Add(OpenList[i]);
            OpenList.Remove(OpenList[i]);
            looped++;
        }
    }

    private void PlacePrefabsOnGrid()
    {
        Buildings = new List<GameObject>();
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                Vector2 pos = m_GridNodes[x, y].WorldPos;
                CityTileType _type = m_GridNodes[x, y].Type;
                if (_type == CityTileType.Street || _type == CityTileType.StreetEnd)
                {
                    Vector3 convertedpos = new Vector3(pos.x, 0, pos.y);
                    Buildings.Add(Instantiate(m_StreetPrefab, convertedpos, Quaternion.identity));
                }
                else if (_type == CityTileType.House)
                {
                    Vector3 convertedpos = new Vector3(pos.x, m_HousePrefab.transform.localScale.y / 2, pos.y);
                    Buildings.Add(Instantiate(m_HousePrefab, convertedpos, Quaternion.identity));
                }
            }
        }
    }

    private void ClearGrid()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                m_GridNodes[x, y].Type = CityTileType.none;
                m_GridNodes[x, y].FollewedStreets = 0;
            }
        }
    }

    private void ClearBuildings()
    {
        if (Buildings != null)
        {
            if (Buildings.Count > 0)
            {
                for (int i = 0; i < Buildings.Count; i++)
                {
                    Destroy(Buildings[i]);
                }
                Buildings.Clear();
            }
        }
    }

    private Vector2Int[] GetNeighBourPointers(Vector2Int Pointer)
    {
        List<Vector2Int> _Neighbours = new List<Vector2Int>();
        Vector2Int[] Pointers = new Vector2Int[]
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
        };

        foreach (Vector2Int i in Pointers)
        {
            int Xpointer = i.x + Pointer.x;
            int Ypointer = i.y + Pointer.y;

            if (Xpointer >= 0 && Ypointer >= 0)
            {
                if (Xpointer < m_GridNodes.GetLength(0) && Ypointer < m_GridNodes.GetLength(1))
                {
                    Vector2Int _PointersFormNeigBour = new Vector2Int(Xpointer, Ypointer);
                    _Neighbours.Add(_PointersFormNeigBour);
                }
            }
        }

        return _Neighbours.ToArray();
    }

    private CityTileType CaculateTile(int followedStreets)
    {
        float followStreetPercent = (2f * followedStreets);
        float StreetThreshHold = 20f + followStreetPercent;
        float HouseTreshHold = 10f - followStreetPercent;
        float _index = Random.Range(0f, 100f);
        if (_index >= StreetThreshHold)
        {
            return CityTileType.Street;
        }
        else
        {
            return CityTileType.House;
        }
    }

    private Vector2Int CaculateFirstStreetPoint()
    {
        int index = Random.Range(0, 100);
        if (index <= 50)
        {
            int X = MinOrMax(0, SizeX - 1);
            int Y = Random.Range(0, SizeY - 1);
            return new Vector2Int(X, Y);
        }
        else
        {
            int X = Random.Range(0, SizeX - 1);
            int Y = MinOrMax(0, SizeY - 1);
            return new Vector2Int(X, Y);
        }
    }

    private int MinOrMax(int min, int max)
    {
        int _index = Random.Range(0, 100);
        if (_index <= 50)
        {
            return min;
        }
        else
        {
            return max;
        }
    }

    private void OnDrawGizmos()
    {
        if (m_GridNodes == null)
        {
            return;
        }

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                Vector2 pos = m_GridNodes[x, y].WorldPos;
                Vector2[] Points = new Vector2[]
                {
                    new Vector2(m_NodeSize /2f,m_NodeSize/2f),
                    new Vector2(-m_NodeSize/2f,m_NodeSize/2f),
                    new Vector2(-m_NodeSize/2f,-m_NodeSize/2f),
                    new Vector2(m_NodeSize/2f,-m_NodeSize/2f),
                };
                for (int i = 0; i < Points.Length; i++)
                {
                    Vector3 pos_1 = Vector3.zero;
                    Vector3 pos_2 = Vector3.zero;

                    if (i == Points.Length - 1)
                    {
                        pos_1 = new Vector3(pos.x, 0, pos.y) + new Vector3(Points[i].x, 0, Points[i].y);
                        pos_2 = new Vector3(pos.x, 0, pos.y) + new Vector3(Points[0].x, 0, Points[0].y);
                    }
                    else
                    {
                        pos_1 = new Vector3(pos.x, 0, pos.y) + new Vector3(Points[i].x, 0, Points[i].y);
                        pos_2 = new Vector3(pos.x, 0, pos.y) + new Vector3(Points[i + 1].x, 0, Points[i + 1].y);
                    }
                    Debug.DrawLine(pos_1, pos_2, Color.blue);
                }

                if (m_GridNodes[x, y].Type == CityTileType.none)
                    Gizmos.color = Color.green;
                else if (m_GridNodes[x, y].Type == CityTileType.StreetEnd)
                    Gizmos.color = Color.red;
                else if (m_GridNodes[x, y].Type == CityTileType.Street)
                    Gizmos.color = Color.blue;
                else if (m_GridNodes[x, y].Type == CityTileType.House)
                    Gizmos.color = Color.cyan;
                Vector3 ConvertePos = new Vector3(pos.x, 0f, pos.y);
                Gizmos.DrawSphere(ConvertePos, 1f);
            }
        }
    }
}

public enum CityTileType
{
    none,
    Street,
    StreetEnd,
    House
}

public struct CityNode
{
    public CityTileType Type;

    public Vector2 WorldPos;

    public int IndexX;
    public int IndexY;

    public int FollewedStreets;
    public bool CanGenerateStreet;
    public Vector2Int PreviousTile;
}