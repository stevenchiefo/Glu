using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Runner : PoolableObject, IEnemy
{
    public int CurrentHealth { get; set; }
    public bool IsAlive { get; set; }
    public EnemyType EnemyType { get; set; }

    private AiWalk m_Aiwalk;

    public override void Load()
    {
        EnemyType = EnemyType.Runner;
        m_Aiwalk = GetComponent<AiWalk>();
        CurrentHealth = GetHealth();
        OnPool.AddListener(ResetHealth);
    }

    public int GetDamage()
    {
        return DataBase.Instance.GetEnemyData(EnemyType).Damage;
    }

    public int GetHealth()
    {
        return DataBase.Instance.GetEnemyData(EnemyType).Health;
    }

    public float GetSpeed()
    {
        return DataBase.Instance.GetEnemyData(EnemyType).Speed;
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
            UIManager.Instance.UpdateUI();
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

    public void Walking(bool boolean)
    {
        m_Aiwalk.SetWalk(boolean);
    }

    
}
