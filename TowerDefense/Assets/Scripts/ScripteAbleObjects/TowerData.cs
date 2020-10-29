using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData")]
public class TowerData : ScriptableObject
{
    public float ShootCD;
    public int MaxLevel;
    public GameObject Projectile;
    public EnemyType[] CantShoot;
}
