using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootAbleData")]
public class ShootAbleData : ScriptableObject
{
    public int Damage;
    public float Speed;
    public bool FollowTarget;
}
