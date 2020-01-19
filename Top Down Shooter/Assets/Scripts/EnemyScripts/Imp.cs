using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    [SerializeField] private GameObject m_Projectile;
    [SerializeField] private float m_CooldownTimer = 0f;
    private float m_Timer = 0f;
    private bool m_Cooldown = false;

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
        AttackSetter();
        if (m_DidHit == true)
        {
            if (m_Cooldown == false)
            {
                m_Cooldown = true;
                Attack();
            }
        }
    }

    private void FixedUpdate()
    {
        ShootRayCast();
    }

    private void AttackSetter()
    {
        m_Timer += Time.deltaTime;
        float endTimer = 1f / m_CooldownTimer;
        if (m_Timer >= endTimer && m_DidHit == true)
        {
            m_Timer = 0;
            Attack();
        }
    }

    protected override void Attack()
    {
        m_Cooldown = true;
        GameObject bullit = Instantiate(m_Projectile, transform.position + new Vector3(0f, 0.5f, 0f), transform.rotation);
        EnemyBullitScript enemyBullit = bullit.GetComponent<EnemyBullitScript>();
        enemyBullit.SetDirection(DirectionToPlayer);
    }
}