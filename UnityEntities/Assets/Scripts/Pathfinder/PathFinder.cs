using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using ChiefoUtilities;
using ChiefoUtilities.PathFinding;

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance;

    private const int m_MOVE_STRAIGHT_COST = 10;
    private const int m_MOVE_Diagnol_COST = 15;

    [SerializeField] private int m_SizeX, m_SizeY;
    [SerializeField] private float m_NodeSize;
    [SerializeField] private LayerMask m_Mask;

    [SerializeField] private bool ShowGrid;
    private ChiefGrid m_Grid;

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
        m_Grid = new ChiefGrid(transform.position, new Vector2(m_SizeX, m_SizeY), m_NodeSize, m_Mask, true);
    }

    public List<PathNode> FindPath(float3 startPoint, float3 EndPoint)
    {
        return ChiefPathFinder.FindPath(m_Grid, startPoint, EndPoint);
    }

    public List<PathNode> GetRandomPath(float3 startPoint)
    {
        return ChiefPathFinder.FindPath(m_Grid, startPoint, m_Grid.GetRandomWalkablePathNode().WorldPosition);
    }

    private void OnDrawGizmos()
    {
        if (m_Grid != null && ShowGrid)
            m_Grid.DrawGizmos();
    }
}