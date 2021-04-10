using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

public class UnitMovementHandler : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<Entity> entities = GetEntityQuery(typeof(UnitData)).ToEntityArray(Allocator.Temp);

        MovementHandle handle = new MovementHandle
        {
            DeltaTime = Time.DeltaTime,
            StopDistance = 0,
        };

        entities.Dispose();

        JobHandle jobHandle = handle.Schedule(this, inputDeps);

        return jobHandle;
    }

    private struct MovementHandle : IJobForEachWithEntity<Translation, UnitData, Rotation>
    {
        public float DeltaTime;
        public float StopDistance;

        public void Execute(Entity entity, int index, ref Translation _Trans, ref UnitData _Data, ref Rotation _rot)
        {
            if (_Data.HasTarget)
            {
                _Data.MovementDirection = math.normalize(_Data.TargetPosition - _Trans.Value);
                _Data.MovementDirection.y = 0f;

                float3 _futurepos = _Trans.Value + (_Data.MovementDirection * _Data.Speed) * DeltaTime;
                _Trans.Value = _futurepos;
            }

            quaternion quaternion = _rot.Value;
            quaternion.value.x = 0;
            quaternion.value.z = 0;
            _rot.Value = quaternion;
        }
    }
}