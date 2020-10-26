using System.Collections;
using UnityEngine;

public enum EnemyType
{
    Type1,
    Type2,
}

public class EnemyController : PoolableObject, IDamageable
{
    public GameObject powerUp;
    public GameObject explosion;
    public GameObject bullet;
    public float minReloadTime = 1.0f;
    public float maxReloadTime = 2.0f;

    public int Health { get; private set; }
    public bool IsAlive { get; set; }

    public float m_Speed;

    private EnemyType m_EnemyType;

    private Vector2 m_Direction;
    private ObjectPool m_BulletPool;
    private float m_Timer;

    [SerializeField] private int m_MaxShootCoolDown;
    [SerializeField] private int m_MinShootCoolDown;
    [SerializeField] private Transform m_Turret;
    private float Timer;
    private float m_CooldownTimer;

    private void Start()
    {
        OnPool.AddListener(SpawnExplosion);
        OnPool.AddListener(SetDead);
        OnPool.AddListener(PoolObject);
        m_BulletPool = gameObject.AddComponent<ObjectPool>();
        m_BulletPool.BeginPool(bullet, 10, null);
    }

    private void Update()
    {
        Move();
        CheckForOutOfBounds();
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(IsAlive);
        }

        m_Timer += Time.deltaTime;
        if (m_Timer >= m_CooldownTimer)
        {
            m_Timer = 0f;
            Shoot();
        }
    }

    private void CheckForOutOfBounds()
    {
        if (!BoundaryManager.Instance.WithinBoundary(transform.position.x, transform.position.y))
        {
            PoolObject();
        }
    }

    private IEnumerator DirectionTimer()
    {
        while (true)
        {
            switch (m_EnemyType)
            {
                case EnemyType.Type1:
                    m_Direction = Vector2.up;
                    StopCoroutine(DirectionTimer());
                    break;

                case EnemyType.Type2:
                    m_Direction = new Vector2(Mathf.RoundToInt(Random.Range(-1f, 1f)), 1f);
                    break;

                default:
                    break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void Shoot()
    {


        PoolableObject poolable = m_BulletPool.GetObject();
        poolable.SpawnObject(m_Turret.transform.position, m_Turret.transform.rotation);
        m_CooldownTimer = Random.Range(m_MinShootCoolDown, m_MaxShootCoolDown);

    }

    private void Move()
    {
        if (!BoundaryManager.Instance.WithinBoundaryX(transform.position.x))
        {
            m_Direction = new Vector2(-m_Direction.x, m_Direction.y);
        }
        transform.Translate(m_Direction * m_Speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
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
        StopCoroutine(DirectionTimer());

        IsAlive = false;
    }

    public void SetType(EnemyType enemyType)
    {
        m_EnemyType = enemyType;
    }

    public override void SpawnObject(Vector2 position, Quaternion quaternion)
    {
        base.SpawnObject(position, quaternion);
        StartCoroutine(DirectionTimer());

    }

    protected override void ResetObject()
    {
        IsAlive = true;
        m_CooldownTimer = Random.Range(m_MinShootCoolDown, m_MaxShootCoolDown);
    }
}