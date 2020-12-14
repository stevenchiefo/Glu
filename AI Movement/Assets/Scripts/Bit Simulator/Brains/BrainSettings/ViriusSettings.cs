using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ViriusSettings",menuName = "ViriusBrainSettings",order = 1)]
public class ViriusSettings : ScriptableObject
{
    [Header("Manual")]
    public BehaviorEnum m_IdleBehavoir;

    public BehaviorEnum m_HuntForMemoryBehavoir;
    public float HuntForMemoryDistance;

    public BehaviorEnum m_SearchForProssecorTreeBehavoir;
    public float SearchForProssecorTreeDistance;

    public BehaviorEnum m_RunFromEnemyBehavoir;
    public float RunFromEnemyDistance;

    public float MatingDistanceNeeded;

    [Header("Stats")]
    public float MaxSpeed;
    public float m_MaxMb;
    public float m_MaxProcess;

    [Header("Cooldowns")]
    public float MBtimer;
    public float ProcessTimer;
    public float LifeTimeNeeded;

    [Header("LayerMasks")]
    public LayerMask MemoryLayerMask;
    public LayerMask MatingLayerMask;
    public LayerMask ProcessTreeLayerMask;
    public LayerMask RunFromEnemyLayerMask; 

}

public enum ViriusType
{
    DataStealer = 0,
    Reaper,
    Core,
}

public enum ViriusMode
{
    Idle = 0,
    HuntForMemory,
    SearchForProcessorTree,
    RunFromEnemys,
}




