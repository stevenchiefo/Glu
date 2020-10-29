using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData")]
public class EnemyData : ScriptableObject
{
    public int Health;
    public float Speed;
    public int Damage;
}
