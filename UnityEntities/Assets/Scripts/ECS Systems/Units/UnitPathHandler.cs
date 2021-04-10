using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

public class UnitPathHandler : ComponentSystem
{
    protected override void OnStartRunning()
    {
        NativeArray<Entity> entities = GetEntityQuery(typeof(UnitData)).ToEntityArray(Allocator.Temp);
        foreach (Entity item in entities)
        {
            EntityManager.AddBuffer<float3BufferData>(item);
        }
        entities.Dispose();
    }

    protected override void OnUpdate()
    {
        Entities.WithAll<UnitData, UnitPathData, PathFindingParams>().ForEach((Entity entity, DynamicBuffer<float3BufferData> Path, ref UnitData _unitdata, ref UnitPathData _pathData, ref PathFindingParams pathFindingParams, ref Translation _trans) =>
          {
              List<PathNode> nodes = PathFinder.Instance.FindPath(_trans.Value, pathFindingParams.EndPos).PathNodes;
              if (nodes != null)
              {
                  Path.Clear();
                  for (int i = 0; i < nodes.Count; i++)
                  {
                      Path.Add(new float3BufferData
                      {
                          Position = nodes[i].Worldposition,
                      });
                  }
              }
              _pathData.Index = 0;
              EntityManager.RemoveComponent<PathFindingParams>(entity);
          });

        Entities.ForEach((Entity entity, DynamicBuffer<float3BufferData> Path, ref UnitData _unitdata, ref UnitPathData _pathData, ref Translation _trans) =>
        {
            if (Path.Length > 0 && _unitdata.HasTarget)
            {
                float distance = math.distance(_unitdata.TargetPosition, _trans.Value);
                if (distance <= 0.2f)
                {
                    _unitdata.TargetPosition = new float3(Path[_pathData.Index].Position.x, _trans.Value.y, Path[_pathData.Index].Position.z);
                    _pathData.Index++;
                    if (_pathData.Index >= Path.Length - 1)
                    {
                        Path.Clear();
                        EntityManager.RemoveComponent<PathFindingParams>(entity);
                        _unitdata.HasTarget = false;
                    }
                }
                else
                {
                    _unitdata.TargetPosition = new float3(Path[_pathData.Index].Position.x, _trans.Value.y, Path[_pathData.Index].Position.z);
                }
            }
        });
    }
}

[InternalBufferCapacity(100)]
public struct float3BufferData : IBufferElementData
{
    public float3 Position;
}