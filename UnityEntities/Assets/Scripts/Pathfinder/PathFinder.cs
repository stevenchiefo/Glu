using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance;

    private const int m_MOVE_STRAIGHT_COST = 10;
    private const int m_MOVE_Diagnol_COST = 15;

    [SerializeField] private int m_SizeX, m_SizeY;
    private Grid m_Grid;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        m_Grid = new Grid(m_SizeX, m_SizeY, transform.position);
    }

    public Path FindPath(float3 startPoint, float3 EndPoint)
    {
        PathNode endnode = m_Grid.GetPathNode(ConvertFloat3ToVector3(EndPoint));

        List<PathNode> OpenList = new List<PathNode>();
        List<PathNode> ClosedList = new List<PathNode>();

        OpenList.Add(m_Grid.GetPathNode(ConvertVector3ToFloat3(startPoint)));

        for (int x = 0; x < m_SizeX; x++)
        {
            for (int z = 0; z < m_SizeY; z++)
            {
                PathNode pathNode = m_Grid.PathNodes[x, z];
                pathNode.CostG = int.MaxValue;
                pathNode.CostH = CaculateDistance(m_Grid.GetPathNode(ConvertFloat3ToVector3(startPoint)), m_Grid.GetPathNode(ConvertFloat3ToVector3(EndPoint)));
                pathNode.CostF = CacaluteFCost(pathNode.CostG, pathNode.CostH);

                pathNode.HasParent = false;
                pathNode.ParentIndex = Vector2.zero;
                m_Grid.PathNodes[x, z] = pathNode;
            }
        }

        while (OpenList.Count > 0)
        {
            PathNode currentNode = GetLowestFcost(OpenList);
            if (IsSameNode(currentNode, endnode))
            {
                return CaculatePath(currentNode);
            }

            List<PathNode> Neighbours = m_Grid.GetNeigbours(currentNode);
            for (int i = 0; i < Neighbours.Count; i++)
            {
                PathNode neighbour = Neighbours[i];
                if (ClosedList.Contains(neighbour))
                    continue;
                if (neighbour.IsWalkable == true)
                {
                    ClosedList.Add(neighbour);
                    continue;
                }

                int tenativeGCost = currentNode.CostG + CaculateDistance(currentNode, neighbour);
                if (tenativeGCost < neighbour.CostG)
                {
                    neighbour.ParentIndex = new Vector2(currentNode.X, currentNode.Z);
                    neighbour.HasParent = true;
                    neighbour.CostG = tenativeGCost;
                    neighbour.CostH = CaculateDistance(neighbour, endnode);
                    neighbour.CostF = CacaluteFCost(neighbour.CostG, neighbour.CostH);

                    if (!OpenList.Contains(neighbour))
                    {
                        OpenList.Add(neighbour);
                    }
                }
                m_Grid.PathNodes[neighbour.X, neighbour.Z] = neighbour;
            }

            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);
        }
        return default;
    }

    private Path CaculatePath(PathNode endnode)
    {
        List<PathNode> nodes = new List<PathNode>();
        nodes.Add(endnode);
        PathNode currentnode = nodes[0];
        while (currentnode.HasParent)
        {
            if (!nodes.Contains(currentnode))
            {
                nodes.Add(m_Grid.PathNodes[(int)currentnode.ParentIndex.x, (int)currentnode.ParentIndex.y]);
                currentnode = m_Grid.PathNodes[(int)currentnode.ParentIndex.x, (int)currentnode.ParentIndex.y];
            }
        }
        nodes.Reverse();

        return new Path
        {
            PathNodes = nodes
        };
    }

    private bool IsSameNode(PathNode A, PathNode B)
    {
        return A.X == B.X && A.Z == B.Z;
    }

    private void OnDrawGizmos()
    {
        if (m_Grid != null)
            m_Grid.OnDrawDebug();
    }

    private PathNode GetLowestFcost(List<PathNode> pathNodes)
    {
        PathNode _bestpathnode = pathNodes[0];
        foreach (PathNode item in pathNodes)
        {
            if (item.CostF < _bestpathnode.CostF)
            {
                _bestpathnode = item;
            }
        }
        return _bestpathnode;
    }

    private int CaculateDistance(PathNode pathNode1, PathNode pathNode2)
    {
        int Xdistance = math.abs(pathNode1.X - pathNode2.X);
        int Zdistance = math.abs(pathNode1.Z - pathNode2.Z);
        int Remaing = math.abs(pathNode1.X - pathNode2.Z);
        return m_MOVE_Diagnol_COST * math.min(Xdistance, Zdistance) + m_MOVE_STRAIGHT_COST * Remaing;
    }

    private int CacaluteFCost(int gCost, int hCost)
    {
        return gCost * hCost;
    }

    private float3 ConvertVector3ToFloat3(Vector3 vector3)
    {
        return new float3(vector3.x, vector3.y, vector3.z);
    }

    private float3 ConvertFloat3ToVector3(float3 float3)
    {
        return new Vector3(float3.x, float3.y, float3.z);
    }
}

public struct Path
{
    public List<PathNode> PathNodes;
}