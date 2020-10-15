using System.Collections;
using UnityEngine;

public class EnemyController : PoolableObject, IDamageable
{
	public GameObject powerUp;
	public GameObject explosion;
	public GameObject bullet;
	public float minReloadTime = 1.0f;
	public float maxReloadTime = 2.0f;
    
    public int Health { get; private set; }
    public bool IsAlive { get;  set; }

    public float m_Speed;

    private void Start()
    {
        OnPool.AddListener(SpawnExplosion);
        OnPool.AddListener(PoolObject);
        OnPool.AddListener(SetDead);
    }


    private void Update()
    {
        Move();
        CheckForOutOfBounds();
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(IsAlive);
        }
    }

    private void CheckForOutOfBounds()
    {
        if (!BoundaryManager.Instance.WithinBoundary(transform.position.x, transform.position.y))
        {
            PoolObject();
        }
    }
    private void Move()
    {
		transform.Translate(Vector2.up * m_Speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            OnPool.Invoke();
        }
    }

    private void SpawnExplosion()
    {
        ExpolsionPool.Instance.SpawnExplosion(transform.position);
    }

    private void SetDead()
    {
        IsAlive = false;
    }
    protected override void ResetObject()
    {
        IsAlive = true;
    }
}
