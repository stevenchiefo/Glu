using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Flyer : PoolableObject, IEnemy
{
    public bool IsAlive { get; set; }
    public int CurrentHealth { get; set; }
    public EnemyType EnemyType { get; set; }

    private bool DoFly;
    private Rigidbody m_Rigidbody;
    private Vector3 m_FlyDirection;


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

    public void Walking(bool boolean)
    {
        DoFly = boolean;
    }

    public override void Load()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        EnemyType = EnemyType.Runner;
        DoFly = false;
        CurrentHealth = GetHealth();
        OnPool.AddListener(ResetHealth);
    }

    private void FixedUpdate()
    {
        if (DoFly)
        {
            Fly();
        }
        
    }
    private void Fly()
    {
        m_Rigidbody.MovePosition(transform.position + m_FlyDirection * GetSpeed() * Time.deltaTime);
    }

    protected override void ResetObject()
    {
        IsAlive = true;
        CurrentHealth = DataBase.Instance.GetEnemyData(EnemyType).Health;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    public override void SpawnObject(Vector3 position)
    {
        base.SpawnObject(position);
        IsAlive = true;
        m_FlyDirection = Finish.Instance.transform.position - transform.position;
        m_FlyDirection = new Vector3(m_FlyDirection.x, 0, 0);
        m_FlyDirection = m_FlyDirection.normalized;
    }

    private void ResetHealth()
    {
        CurrentHealth = DataBase.Instance.GetEnemyData(EnemyType).Health;
    }
}
