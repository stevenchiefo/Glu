using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntiyManager : MonoBehaviour
{
    [SerializeField] private GameObject m_NormalEnemy;
    [SerializeField] private GameObject m_EliteEnemy;
    [SerializeField] private GameObject m_BigEnemy;
    [SerializeField] private int m_UpgradeCooldown;
    [SerializeField] private GameObject m_UpgradePrefab;

    [SerializeField] private Boundary m_TopBoundery;

    public int LevelIndex = 0;
    private EnemyWave[] EnemySpawns;
    private bool CanSpawnEnemys;

    private delegate void EnityManagerTask();

    private Collider2D Collider2D;
    private List<IDamageable> SpawnedEnemys;

    private ObjectPool m_NormalPool;
    private ObjectPool m_UpgradePool;

    private void Start()
    {
        SetSpawns();
        StartCoroutine(CheckSpawnEnemys());
    }

    private void SetSpawns()
    {
        Collider2D = GetComponent<Collider2D>();
        SpawnedEnemys = new List<IDamageable>();
        m_NormalPool = gameObject.AddComponent<ObjectPool>();
        m_UpgradePool = gameObject.AddComponent<ObjectPool>();
        m_UpgradePool.BeginPool(m_UpgradePrefab, 5, transform);
        m_NormalPool.BeginPool(m_NormalEnemy, 5, transform);

        EnemySpawns = new EnemyWave[]
        {
            new EnemyWave
            {
                Normal = 3,
                Elite = 3,
            },
            new EnemyWave
            {
                Normal = 5,
            },
            new EnemyWave
            {
                Normal = 6,
            }
        };
        StartCoroutine(SpawnUpgradeBall());
    }

    private IEnumerator CheckSpawnEnemys()
    {
        while (true)
        {
            yield return new WaitUntil(() => AllEnemysDead() == true);
            SpawnEnemys();
        }
    }

    private void SpawnEnemys()
    {
        SpawnedEnemys.Clear();

        if (LevelIndex > EnemySpawns.Length - 1)
        {
            GameManager.Instance.ShowGameOver();
            return;
        }

        int indexNormal = EnemySpawns[LevelIndex].Normal;

        for (int i = 0; i < indexNormal; i++)
        {
            PoolableObject poolableObject = m_NormalPool.GetObject();
            Quaternion rotation = new Quaternion(0f, 0f, 180f, 0f);
            poolableObject.SpawnObject(GetRandompos(), rotation);
            EnemyController enemyController = poolableObject.GetComponent<EnemyController>();
            enemyController.SetType(EnemyType.Type1);
            SpawnedEnemys.Add(poolableObject.GetComponent<IDamageable>());
        }

        int indexElite = EnemySpawns[LevelIndex].Elite;
        for (int i = 0; i < indexElite; i++)
        {
            PoolableObject poolableObject = m_NormalPool.GetObject();
            Quaternion rotation = new Quaternion(0f, 0f, 180f, 0f);
            poolableObject.SpawnObject(GetRandompos(), rotation);
            EnemyController enemyController = poolableObject.GetComponent<EnemyController>();
            enemyController.SetType(EnemyType.Type2);
            SpawnedEnemys.Add(poolableObject.GetComponent<IDamageable>());
        }
        LevelIndex++;
    }

    private IEnumerator SpawnUpgradeBall()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_UpgradeCooldown);
            PoolableObject poolableObject = m_UpgradePool.GetObject();
            Quaternion rotation = new Quaternion(0f, 0f, 180f, 0f);
            poolableObject.SpawnObject(GetRandompos(), rotation);
        }
    }

    private Vector2 GetRandompos()
    {
        float x = UnityEngine.Random.Range(-Collider2D.bounds.size.x / 2f, Collider2D.bounds.size.x / 2f);
        float y = UnityEngine.Random.Range(0, 5f);
        return new Vector2(x + transform.position.x, y + transform.position.y);
    }

    private bool AllEnemysDead()
    {
        if (SpawnedEnemys.Count == 0)
        {
            return true;
        }
        int amountdead = 0;
        for (int i = 0; i < SpawnedEnemys.Count; i++)
        {
            if (SpawnedEnemys[i].IsAlive == false)
            {
                amountdead++;
            }
        }
        return amountdead == SpawnedEnemys.Count;
    }
}

public struct EnemyWave
{
    public int Normal;
    public int Elite;
    public int Big;
}