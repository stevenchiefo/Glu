using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipData")]
public class ShipData : ScriptableObject
{
    
    public int MaxDurrabilty;
    public float Speed;
    public float RotationSpeed;
    public float FirePower;
    public CannonBall.TargetType TargetType;
}
