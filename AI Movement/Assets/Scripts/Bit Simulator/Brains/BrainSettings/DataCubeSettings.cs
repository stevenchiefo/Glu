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