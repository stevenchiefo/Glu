using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public float drivingForce;
    public float vehicleTurningSpeed;
    public float turretRotationSpeed;
    public float waypointRange;
    public float reloadTime;
    public int score;
}