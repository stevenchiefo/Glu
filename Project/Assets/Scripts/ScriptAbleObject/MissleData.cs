using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissleData")]
public class MissleData : ScriptableObject
{
    public float speed;
    public float lifeTime;
    public float RotationSpeed;
    public float Range;
    public int damage;
}