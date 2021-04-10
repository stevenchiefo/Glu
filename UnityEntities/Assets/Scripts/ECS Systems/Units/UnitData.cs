using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct UnitData : IComponentData
{
    public float3 MovementDirection;
    public float3 FinalTargetPosition;
    public float3 TargetPosition;
    public bool HasTarget;
    public float Speed;
    public bool Selected;
}