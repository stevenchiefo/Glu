using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Runner,
    Flyer
}

public interface IEnemy
{
    bool IsAlive { get; set; }
    int CurrentHealth { get; set; }

    EnemyType EnemyType { get; set; }

    float GetSpeed();

    int GetDamage();

    int GetHealth();

    void TakeDamage(int _Damage);

    Transform GetTarget();

    void Walking(bool boolean);


}
