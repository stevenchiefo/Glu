using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct UnitData : IComponentData
{
    public float3 MovementDirection;
    public float3 TargetPosition;
    public float Speed;
    public bool Selected;
   
    
}
