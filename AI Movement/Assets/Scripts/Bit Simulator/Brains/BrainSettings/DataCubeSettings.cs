using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataCubeSettings", menuName = "BrainSettings", order = 0)]
public class DataCubeSettings : ScriptableObject
{
    [Header("Manual")]
    public BehaviorEnum m_IdleBehavoir;

    public BehaviorEnum m_SearchForMemoryBehavoir;
    public float SearchForMemoryDistance;

    public BehaviorEnum m_SearchForProssecorTreeBehavoir;
    public float SearchForProssecorTreeDistance;

    public BehaviorEnum m_RunFromEnemyBehavoir;
    public float RunFromEnemyDistance;

    public float MatingDistanceNeeded;

    [Header("Stats")]
    public float MaxSpeed;
    public float MaxDefense;
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

public enum DataCubeType
{
    Bit = 0,
    Byte,
    Int,
    Float,
    String,
}



public enum DataCubeMode
{
    Idle,
    SearchForMemory,
    SearchForProcessorTree,
    RunFromEnemys,
}

public enum BehaviorEnum
{
    Keyboard,
    SeekClickPoint,
    Seek,
    Flee,
    Pursue,
    Evade,
    Wander,
    FollowPath,
    Hide,
    NotSet,
    ObjectAvoid,
    Ilde,
};