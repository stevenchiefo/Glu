using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GarbargeCollectorSettings", menuName = "Settings", order = 0)]
public class GarbargeCollectorSettings : ScriptableObject
{
    [Header("Manual")]
    public BehaviorEnum m_IdleBehavoir;

    public BehaviorEnum HuntBehavoir;
    public float HuntRange;

    [Header("Stats")]
    public float NeededMbToHunt;

    public float CollectDistance;

    [Header("LayerMasks")]
    public LayerMask EnemyLayerMask;
}

public enum GarbargeCollectorMode
{
    Idle,
    Hunt,
}