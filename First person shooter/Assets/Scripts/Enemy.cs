using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_Health = 100;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Death();
    }

    public void GotHit(float damage)
    {
        m_Health = m_Health - damage;
        Rigidbody r = gameObject.GetComponent<Rigidbody>();
        //r.AddRelativeForce(Vector3.back * (damage * 10));
    }

    private void Death()
    {
        if (m_Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool IsDeath()
    {
        return m_Health <= 0;
    }
}