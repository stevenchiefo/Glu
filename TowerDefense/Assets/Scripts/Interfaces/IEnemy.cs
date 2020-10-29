using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Runner,
}

public interface IEnemy
{
    bool IsAlive { get; set; }
    int CurrentHealth { get; set; }

    float GetSpeed();

    int GetDamage();

    int GetHealth();

    void TakeDamage(int _Damage);

    Transform GetTarget();

}
