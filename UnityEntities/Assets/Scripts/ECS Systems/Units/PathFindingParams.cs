using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct PathFindingParams : IComponentData
{
    public float3 StartPos;
    public float3 EndPos;
}