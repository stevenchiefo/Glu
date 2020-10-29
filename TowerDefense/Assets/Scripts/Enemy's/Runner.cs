using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Runner : PoolableObject, IEnemy
{
    public int CurrentHealth { get; set; }
    public bool IsAlive { get; set; }

    public override void Load()
    {
        CurrentHealth = GetHealth();
        OnPool.AddListener(ResetHealth);
    }

    public int GetDamage()
    {
        return DataBase.Instance.GetEnemyData(EnemyType.Runner).Damage;
    }

    public int GetHealth()
    {
        return DataBase.Instance.GetEnemyData(EnemyType.Runner).Health;
    }

    public float GetSpeed()
    {
        return DataBase.Instance.GetEnemyData(EnemyType.Runner).Speed;
    }

    public Transform GetTarget()
    {
        return transform;
    }
    

    public void TakeDamage(int _Damage)
    {
        CurrentHealth -= _Damage;
        if (CurrentHealth <= 0)
        {
            IsAlive = false;
            PoolObject();
        }
    }

    protected override void ResetObject()
    {
        IsAlive = true;
        CurrentHealth = DataBase.Instance.GetEnemyData(EnemyType.Runner).Health;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void ResetHealth()
    {
        CurrentHealth = DataBase.Instance.GetEnemyData(EnemyType.Runner).Health;
    }
}
