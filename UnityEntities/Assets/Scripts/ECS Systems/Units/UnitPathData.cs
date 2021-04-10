using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

[GenerateAuthoringComponent]
public struct UnitPathData : IComponentData
{
    public int Index;
}