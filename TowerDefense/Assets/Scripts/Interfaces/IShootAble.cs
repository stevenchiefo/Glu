using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootAbleType
{
    ArcherArrow,
}

public interface IShootAble
{
    Transform Target { get; set; }

    void SetTarget(Transform _Target);

    int GetDamage();

    float GetSpeed();

    void FireObject(Vector3 direction);
}
