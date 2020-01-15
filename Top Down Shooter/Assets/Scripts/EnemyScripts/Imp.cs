using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    [SerializeField] private GameObject m_Projectile;

    private void Awake()
    {
        Load();
    }

    protected override void Move()
    {
    }

    private void Update()
    {
        Updater();
        IsDeath();
        if (Death == true)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        ShootRayCast();
    }

    protected override void Attack()
    {
    }
}