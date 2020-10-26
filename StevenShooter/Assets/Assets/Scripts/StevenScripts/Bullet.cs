using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolableObject
{
    [SerializeField] private float m_Speed;

    private int m_Damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitEnemy(collision);
        CheckForPool(collision);
    }

    private void Update()
    {
        Move();
        CheckForPoolByPosition();
    }

    private void Move()
    {
        transform.Translate(Vector2.up * m_Speed * Time.deltaTime);

    }

    private void CheckForPoolByPosition()
    {
        if (!BoundaryManager.Instance.WithinBoundary(transform.position.x, transform.position.y))
        {
            PoolObject();
        }

    }

    private void HitEnemy(Collider2D collision2D)
    {
        IDamageable damageable = collision2D.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(m_Damage);
            PoolObject();
        }
    }

    private void CheckForPool(Collider2D collision2D)
    {
        Boundary boundary = collision2D.gameObject.GetComponent<Boundary>();
        if (boundary != null)
        {
            PoolObject();
        }
    }
}
