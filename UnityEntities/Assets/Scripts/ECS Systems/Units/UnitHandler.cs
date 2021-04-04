using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

public class UnitHandler : ComponentSystem
{
    private float StopDistance = 5f;

    protected override void OnStartRunning()
    {
        base.OnStartRunning();
    }

    protected override void OnUpdate()
    {
        CheckMovement();
    }

    private void CheckSelection()
    {
        if (MouseSelecter.Instance.HasASelection())
        {
            Vector3[] Selection = MouseSelecter.Instance.CheckForSelection();
            
        }
    }

    private void CheckMovement()
    {
        Entities.WithAll<UnitData>().ForEach((ref UnitData _Data, ref Translation _Trans) =>
        {
            _Data.MovementDirection = math.normalize(_Data.TargetPosition - _Trans.Value);
        });

        Entities.WithAll<UnitData>().ForEach((ref UnitData _Data,ref Translation _Trans) =>
        {
            float _distance = math.distance(_Trans.Value, _Data.TargetPosition);
            if (_distance > StopDistance)
            {
                float3 _futurepos = _Trans.Value + (_Data.MovementDirection * _Data.Speed) * Time.DeltaTime;
                _Trans.Value = _futurepos;
            }
        });
    }
}
