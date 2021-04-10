using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

public class UnitHandler : ComponentSystem
{
    private float StopDistance = 0f;
    private QuadTree m_Tree;

    protected override void OnCreate()
    {
        m_Tree = new QuadTree(200, 10, new Vector3(20, 0, -20));
    }

    protected override void OnUpdate()
    {
        CheckSelection();
        CheckTarget();
        //CheckMovement();
        PutInQuadTree();
    }

    private void PutInQuadTree()
    {
        NativeArray<Entity> entities = GetEntityQuery(typeof(UnitData)).ToEntityArray(Allocator.Temp);
        List<Point> _points = new List<Point>();
        for (int i = 0; i < entities.Length; i++)
        {
            Translation _trans = EntityManager.GetComponentData<Translation>(entities[i]);
            Point point = new Point
            {
                WorldPosition = new Vector3(_trans.Value.x, 0, _trans.Value.z),
                Speed = 0,
            };
            _points.Add(point);
        }
        m_Tree.SpawnPoints(_points);
        entities.Dispose();

        MouseSelecter.Instance.SetTree(m_Tree);
    }

    private void CheckSelection()
    {
        if (MouseSelecter.Instance.HasASelection())
        {
            Vector3[] positions = MouseSelecter.Instance.CheckForSelection();
            Vector3[] Selection = MouseSelecter.Instance.GetCorners(positions[0], positions[1]);

            Entities.WithAll<UnitData>().ForEach((ref UnitData _data, ref Translation _Trans) =>
            {
                if (IsWithinSelection(Selection, _Trans.Value))
                {
                    _data.Selected = true;
                }
                else
                {
                    _data.Selected = false;
                }
            });

            MouseSelecter.Instance.ResetSelection();
        }
    }

    private void CheckTarget()
    {
        if (MouseSelecter.Instance.DidSelectTarget())
        {
            Vector3 _pos = MouseSelecter.Instance.GetTargetPosition();
            float3 _ConvertedPos = new float3(_pos.x, 0, _pos.z);
            int amount = 0;
            Entities.WithAll<UnitData>().ForEach((ref UnitData _data) =>
            {
                if (_data.Selected)
                {
                    _data.HasTarget = true;
                    _data.FinalTargetPosition = _ConvertedPos;
                    amount++;
                }
            });
            int RowsThressHold = (int)(math.sqrt(amount * math.PI) / 2f);

            NativeArray<Entity> entities = GetEntityQuery(typeof(UnitData)).ToEntityArray(Allocator.Temp);
            List<Entity> _localEntites = new List<Entity>();
            for (int i = 0; i < entities.Length; i++)
            {
                UnitData _currentData = EntityManager.GetComponentData<UnitData>(entities[i]);
                if (_currentData.Selected)
                {
                    _localEntites.Add(entities[i]);
                }
            }
            float3 offsetFloat = float3.zero;
            float3 Offset = new float3((-1.5f * RowsThressHold) / 2f, 0, (-1.5f * RowsThressHold) / 2f);
            int offsetCounter = 0;
            for (int i = 0; i < _localEntites.Count; i++)
            {
                if (offsetCounter >= RowsThressHold)
                {
                    offsetCounter = 0;
                    offsetFloat += new float3(0, 0, 1.5f);
                }
                UnitData _Data = EntityManager.GetComponentData<UnitData>(_localEntites[i]);
                _Data.FinalTargetPosition = _ConvertedPos + new float3(1.5f * offsetCounter, 0, 0) + offsetFloat + Offset;
                EntityManager.SetComponentData(_localEntites[i], _Data);
                EntityManager.AddComponentData(_localEntites[i], new PathFindingParams
                {
                    StartPos = EntityManager.GetComponentData<Translation>(_localEntites[i]).Value,
                    EndPos = _Data.FinalTargetPosition,
                });

                offsetCounter++;
            }
        }
    }

    private bool IsWithinSelection(Vector3[] _positions, float3 _pos)
    {
        if (_positions[3].x <= _pos.x && _positions[0].x >= _pos.x)
        {
            if (_positions[3].z <= _pos.z && _positions[0].z >= _pos.z)
            {
                return true;
            }
        }
        return false;
    }
}