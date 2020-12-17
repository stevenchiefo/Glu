using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolableObject
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_MaxSpeed;
    private Rigidbody m_Rb;

    public bool Dead;

    // Start is called before the first frame update
    public override void Load()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MoveToPlayer();
        CheckForDead();
    }

    private void CheckForDead()
    {
        if (transform.position.y < -10)
        {
            Dead = true;
            PoolObject();
        }
    }

    private void MoveToPlayer()
    {
        Vector3 dir = Player.instance.transform.position - transform.position;
        m_Rb.AddForce(dir.normalized * m_Speed * Time.deltaTime, ForceMode.Acceleration);
        m_Rb.velocity = Vector3.ClampMagnitude(m_Rb.velocity, m_MaxSpeed);
    }

    protected override void ResetObject()
    {
        Dead = false;
        m_Rb.velocity = Vector3.zero;
    }
}