using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Grid
{
    public PathNode[,] PathNodes { get; set; }
    private int m_SizeX;
    private int m_SizeZ;
    private Vector3 m_WorldPosition;

    public Grid(int SizeX, int SizeY, Vector3 worldPosition)
    {
        m_SizeX = SizeX;
        m_SizeZ = SizeY;
        m_WorldPosition = worldPosition;
        CreateGrid();
    }

    public PathNode GetPathNode(Vector3 WorldPosition)
    {
        int indexX = Mathf.RoundToInt(WorldPosition.x);
        int indexZ = Mathf.RoundToInt(WorldPosition.z);

        if (indexX >= 0 && indexX < m_SizeX)
        {
            if (indexZ >= 0 && indexX < m_SizeZ)
                return PathNodes[indexX, indexZ];
        }
        return default;
    }

    private void CreateGrid()
    {
        PathNodes = new PathNode[m_SizeX, m_SizeZ];
        for (int x = 0; x < m_SizeX; x++)
        {
            for (int z = 0; z < m_SizeZ; z++)
            {
                PathNode pathNode = new PathNode
                {
                    X = x,
                    Z = z,

                    Worldposition = new float3(x, 0, z) + new float3(m_WorldPosition.x, m_WorldPosition.y, m_WorldPosition.z),

                    CostF = int.MaxValue,
                    CostG = int.MaxValue,
                    CostH = int.MaxValue,

                    IsWalkable = CheckIfWalkable(x, z),
                    ParentIndex = Vector2.zero,
                };
                PathNodes[x, z] = pathNode;
            }
        }
    }

    public List<PathNode> GetNeigbours(PathNode pathNode)
    {
        List<PathNode> nodes = new List<PathNode>();
        Vector2Int[] Directions = new Vector2Int[]
        {
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
        };

        foreach (Vector2Int dir in Directions)
        {
            int PointX = dir.x + pathNode.X;
            int PointZ = dir.y + pathNode.Z;

            if (PointX >= 0 && PointX < m_SizeX)
            {
                if (PointZ >= 0 && PointZ < m_SizeZ)
                {
                    nodes.Add(PathNodes[PointX, PointZ]);
                }
            }
        }
        return nodes;
    }

    private bool CheckIfWalkable(int X, int Z)
    {
        Collider[] _cols = Physics.OverlapBox(new Vector3(X, 0, Z) + m_WorldPosition, new Vector3(0.1f, 0.1f, 0.1f));
        return _cols.Length == 0;
    }

    public void OnDrawDebug()
    {
        if (PathNodes != null)
        {
            for (int x = 0; x < m_SizeX; x++)
            {
                for (int z = 0; z < m_SizeZ; z++)
                {
                    PathNode pathNode = PathNodes[x, z];

                    Color color = Color.gray;
                    if (pathNode.IsWalkable == false)
                    {
                        color = Color.red;
                    }
                    Gizmos.color = color;
                    Gizmos.DrawCube(new Vector3(pathNode.X, 0f, pathNode.Z) + m_WorldPosition, new Vector3(0.5f, 0.5f, 0.5f));
                }
            }
        }
    }
}

public struct PathNode
{
    public int X;
    public int Z;

    public float3 Worldposition;

    public int CostH;
    public int CostG;
    public int CostF;

    public bool IsWalkable;

    public bool HasParent;
    public Vector2 ParentIndex;
}