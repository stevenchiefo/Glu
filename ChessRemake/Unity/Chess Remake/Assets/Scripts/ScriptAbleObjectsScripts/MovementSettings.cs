using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings",menuName = "Settings/MovementSettings",order = 0)]
public class MovementSettings : ScriptableObject
{
    public float GeneralSpeed;
    public float LockRange;
}
