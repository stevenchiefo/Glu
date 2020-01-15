using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health;
    public string Name;
    public float Level;

    private void Start()
    {
        float multiplyer = Level / 10f + Level;
        Health = Health * multiplyer;
        Health = Mathf.Round(Health);
    }

    protected virtual void Attack()
    {
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    protected void IsDeath()
    {
        if (Health < 0)
        {
            Destroy(gameObject);
        }
    }
}