using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitySettings", menuName = "Entity/Settings", order = 0)]
public class EntitySettings : ScriptableObject
{
    [Header("Common Stats")]
    public float MinMaxHealth;

    public float MaxMaxHealth;

    public float MinDetectionRange, MaxDetectionRange;

    public float MinHungerTreshHold, MaxHungerTreshHold;

    public float MinSpeed, MaxSpeed;

    [Header("RelationShip Stats")]
    public float MinApeal;

    public float MaxApeal;

    public float MinApealTreshHold, MaxApealTreshHold;

    public float MinSexualTreshHold, MaxSexualTreshHold;

    public float MinLoyalty, MaxLoyalty;

    public float MinConfidence, MaxConfidence;

    public float MinAproachable, MaxAproachable;

    public float MinSparkTreshHold, MaxSprakTreshHold;
}