using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnitiyManager : MonoBehaviour
{
    [Header("Enemy Spawn values")]
    [SerializeField] private GameObject m_EnemyPrefabs;

    [SerializeField] private int m_AmountPoints;
    [SerializeField] private Vector3 m_MinPos;
    [SerializeField] private Vector3 m_MaxPos;

    //Points
    private Vector3[] m_SpawnPoints;

    [Header("PowerUp Values")]
    [SerializeField] private GameObject[] m_PowerUps;

    [SerializeField] private float m_PowerUpTimer;
    private PowerUp m_ActivePowerUp;

    [Header("Detection")]
    [SerializeField] private LayerMask m_PlayerLayer;

    [SerializeField] private float m_DetectionRadius;

    //Pool
    private ObjectPool m_EnemiesPool;

    private ObjectPool[] m_PowerUpPools;

    [Header("Waves and Enemys Spawn")]
    [SerializeField] private List<int> m_EnemyWaves;

    private int m_Index;

    private List<Enemy> m_EnemiesAlive;

    private void Start()
    {
        SetPoints();
        MakePool();
        StartCoroutine(SpawnWaves());
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitUntil(() => CanSpawn());
            if (m_Index < m_EnemyWaves.Count)
            {
                SpawnEnemys(m_EnemyWaves[m_Index]);
                m_Index++;
            }
        }
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            yield return new WaitUntil(() => CanSpawnPowerUp());
            yield return new WaitForSeconds(m_PowerUpTimer);
            SpawnPowerUp();
        }
    }

    private bool CanSpawnPowerUp()
    {
        if (m_ActivePowerUp == null)
        {
            return true;
        }
        if (m_ActivePowerUp.PickedUp)
        {
            m_ActivePowerUp = null;
            return true;
        }

        return false;
    }

    private void SpawnPowerUp()
    {
        int index = Random.Range(0, m_PowerUpPools.Length);
        PoolableObject poolableObject = m_PowerUpPools[index].GetObject();
        Vector3 pos = GetRandomPos();
        while (DetectPlayer(pos))
        {
            pos = GetRandomPos();
        }
        poolableObject.SpawnObject(pos);

        m_ActivePowerUp = poolableObject.GetComponent<PowerUp>();
    }

    private void SpawnEnemys(int ammount)
    {
        for (int i = 0; i < ammount; i++)
        {
            int index = Random.Range(0, m_SpawnPoints.Length);
            if (DetectPlayer(m_SpawnPoints[index]))
            {
                index = Random.Range(0, m_SpawnPoints.Length);
            }
            PoolableObject poolableObject = m_EnemiesPool.GetObject();
            poolableObject.SpawnObject(m_SpawnPoints[index]);
            m_EnemiesAlive.Add(poolableObject.GetComponent<Enemy>());
        }
    }

    private void SetPoints()
    {
        m_SpawnPoints = new Vector3[m_AmountPoints];
        for (int i = 0; i < m_SpawnPoints.Length; i++)
        {
            Vector3 pos = GetRandomPos();
            while (DetectPlayer(pos))
            {
                pos = GetRandomPos();
            }
            m_SpawnPoints[i] = pos;
        }
    }

    private bool CanSpawn()
    {
        for (int i = 0; i < m_EnemiesAlive.Count; i++)
        {
            if (m_EnemiesAlive[i].Dead)
            {
                m_EnemiesAlive.RemoveAt(i);
            }
        }
        return m_EnemiesAlive.Count <= 0;
    }

    private void MakePool()
    {
        m_EnemiesAlive = new List<Enemy>();
        m_PowerUpPools = new ObjectPool[m_PowerUps.Length];

        for (int i = 0; i < m_PowerUpPools.Length; i++)
        {
            m_PowerUpPools[i] = gameObject.AddComponent<ObjectPool>();
            m_PowerUpPools[i].BeginPool(m_PowerUps[i], 10, transform);
        }

        m_EnemiesPool = gameObject.AddComponent<ObjectPool>();
        m_EnemiesPool.BeginPool(m_EnemyPrefabs, 3, transform);
    }

    private bool DetectPlayer(Vector3 pos)
    {
        Collider[] collider = Physics.OverlapSphere(pos, m_DetectionRadius, m_PlayerLayer);
        return collider.Length > 0;
    }

    private Vector3 GetRandomPos()
    {
        float X = Random.Range(m_MinPos.x, m_MaxPos.x);
        float Z = Random.Range(m_MinPos.z, m_MaxPos.z);

        return new Vector3(X, 0, Z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 rightmin = new Vector3(m_MinPos.x, 0f, m_MaxPos.z);
        Vector3 rightmax = new Vector3(m_MaxPos.x, 0f, m_MinPos.z);
        Gizmos.DrawLine(m_MinPos, rightmin);
        Gizmos.DrawLine(rightmin, m_MaxPos);
        Gizmos.DrawLine(m_MaxPos, rightmax);
        Gizmos.DrawLine(rightmax, m_MinPos);

        if (m_SpawnPoints != null)
        {
            for (int i = 0; i < m_SpawnPoints.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawIcon(m_SpawnPoints[i], "SpawnPoint", true, Color.red);
            }
        }
    }
}