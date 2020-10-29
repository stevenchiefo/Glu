using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnityManager : MonoBehaviour
{
    public static EnityManager Instance;

    [SerializeField] private WaveData m_WaveData;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] private GameObject m_RunnerPrefab;
    private bool m_CanSpawn;
    private int m_CurrentWave;
    private Dictionary<EnemyType, ObjectPool> m_Pools;
    private List<IEnemy> CurrentAliveEnemys;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        LoadPools();
        StartCoroutine(CheckSpawnEnemies());
    }

    private void Update()
    {
        m_CanSpawn = AllEnemysDead();
    }

    private void LoadPools()
    {
        CurrentAliveEnemys = new List<IEnemy>();
        m_Pools = new Dictionary<EnemyType, ObjectPool>();
        m_Pools.Add(EnemyType.Runner, gameObject.AddComponent<ObjectPool>());
        m_Pools[EnemyType.Runner].BeginPool(m_RunnerPrefab, 10, null);
    }

    private bool AllEnemysDead()
    {
        if (CurrentAliveEnemys != null)
        {
            int ammount = 0;
            for (int i = 0; i < CurrentAliveEnemys.Count; i++)
            {
                if (!CurrentAliveEnemys[i].IsAlive)
                {
                    ammount++;
                }
            }
            return ammount == CurrentAliveEnemys.Count;
        }
        return true;
    }

    private IEnumerator CheckSpawnEnemies()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_CanSpawn);
            if (m_CurrentWave < m_WaveData.Waves)
            {
                StartCoroutine(SpawnEnemys());
            }
            else
            {
                GameManager.Instance.SetGameWon();
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator SpawnEnemys()
    {
        CurrentAliveEnemys.Clear();
        for (int i = 0; i < m_WaveData.m_RunnerAmmount[m_CurrentWave]; i++)
        {
            PoolableObject poolableObject = m_Pools[EnemyType.Runner].GetObject();
            poolableObject.SpawnObject(m_SpawnPoint.position);
            IEnemy enemy = poolableObject.GetComponent<IEnemy>();
            CurrentAliveEnemys.Add(enemy);
            yield return new WaitForSeconds(1.5f);
        }
        m_CurrentWave++;
    }

    public (int currentWave, int TotalWaves) GetWaveInfo()
    {
        return (m_CurrentWave, m_WaveData.Waves);
    }
}