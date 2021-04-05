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

    protected override void OnUpdate()
    {
        CheckSelection();
        CheckTarget();
        CheckMovement();
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
                    _data.TargetPosition = _ConvertedPos;
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
                _Data.TargetPosition = _ConvertedPos + new float3(1.5f * offsetCounter, 0, 0) + offsetFloat + Offset;
                EntityManager.SetComponentData(_localEntites[i], _Data);

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

    private void CheckMovement()
    {
        Entities.WithAll<UnitData>().ForEach((ref UnitData _Data, ref Translation _Trans) =>
        {
            if (_Data.HasTarget)
            {
                _Data.MovementDirection = math.normalize(_Data.TargetPosition - _Trans.Value);
                _Data.MovementDirection.y = 0f;
            }
        });

        Entities.WithAll<UnitData>().ForEach((ref UnitData _Data, ref Translation _Trans, ref Rotation _rot) =>
        {
            float _distance = math.distance(_Trans.Value, _Data.TargetPosition);
            if (_distance > StopDistance)
            {
                float3 _futurepos = _Trans.Value + (_Data.MovementDirection * _Data.Speed) * Time.DeltaTime;
                _Trans.Value = _futurepos;
            }
            quaternion quaternion = _rot.Value;
            quaternion.value.x = 0;
            quaternion.value.z = 0;
            _rot.Value = quaternion;
        });
    }
}