using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerMoveData : IComponentData
{
    [Header("Input Keys")]
    public KeyCode Forward;

    public KeyCode Backward;
    public KeyCode Right;
    public KeyCode Left;

    [Header("Speed")]
    public float Speed;
}