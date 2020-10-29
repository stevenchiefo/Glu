using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnityManager : MonoBehaviour
{
    public static EnityManager Instance;

    [SerializeField] private WaveData m_WaveData;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] private GameObject m_RunnerPrefab;
    [SerializeField] private GameObject m_FlyerPrefab;
    private bool m_CanSpawn;
    private int m_CurrentWave;
    private Dictionary<EnemyType, ObjectPool> m_Pools;
    private List<IEnemy> CurrentAliveEnemys;

    private float m_Timer;

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

    }

    private void Update()
    {
        m_CanSpawn = AllEnemysDead();
        CheckSpawnEnemies();
        if (GameManager.Instance.GameOver)
        {
            Instance.enabled = false;
        }
    }

    private void LoadPools()
    {
        CurrentAliveEnemys = new List<IEnemy>();
        m_Pools = new Dictionary<EnemyType, ObjectPool>();
        m_Pools.Add(EnemyType.Runner, gameObject.AddComponent<ObjectPool>());
        m_Pools[EnemyType.Runner].BeginPool(m_RunnerPrefab, 10, null);
        m_Pools.Add(EnemyType.Flyer, gameObject.AddComponent<ObjectPool>());
        m_Pools[EnemyType.Flyer].BeginPool(m_FlyerPrefab, 10, null);
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

    private void CheckSpawnEnemies()
    {


        if (m_CanSpawn)
        {
            m_Timer += Time.deltaTime;

            if (m_Timer >= 1f)
            {
                m_Timer = 0f;


                if (m_CurrentWave < m_WaveData.Waves)
                {
                    SpawnEnemys();
                }
                else
                {
                    GameManager.Instance.SetGameWon();
                    gameObject.SetActive(false);
                }
            }
        }

    }

    private void SpawnEnemys()
    {
        UIManager.Instance.UpdateUI();
        CurrentAliveEnemys.Clear();
        StartCoroutine(SpawnRunnersEnemies());
        StartCoroutine(SpawnFlyersEnemies());


        for (int i = 0; i < CurrentAliveEnemys.Count; i++)
        {
            CurrentAliveEnemys[i].Walking(true);
        }
        m_CurrentWave++;
        UIManager.Instance.UpdateUI();
    }

    private IEnumerator SpawnRunnersEnemies()
    {
        if (m_WaveData.m_RunnerAmmount.Count > 0)
        {


            float _Yoffset = 0f;
            float Xoffset = 0f;
            int WidthLength = 5;
            int currentCount = 0;
            for (int i = 0; i < m_WaveData.m_RunnerAmmount[m_CurrentWave]; i++)
            {
                PoolableObject poolableObject = m_Pools[EnemyType.Runner].GetObject();
                if (currentCount >= WidthLength)
                {
                    currentCount = 0;
                    _Yoffset += 2f;
                    Xoffset = 0f;
                }

                Vector3 _offset = new Vector3(-_Yoffset, 0f, -Xoffset);
                poolableObject.SpawnObject(m_SpawnPoint.position + _offset);
                IEnemy enemy = poolableObject.GetComponent<IEnemy>();
                enemy.Walking(false);
                CurrentAliveEnemys.Add(enemy);
                Xoffset += 2f;
                currentCount++;
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < CurrentAliveEnemys.Count; i++)
            {
                CurrentAliveEnemys[i].Walking(true);
            }
        }
    }
    private IEnumerator SpawnFlyersEnemies()
    {
        if (m_WaveData.m_FlyerAmmount.Count > 0)
        {


            float _Yoffset = 0f;
            float Xoffset = 0f;
            int WidthLength = 5;
            int currentCount = 0;
            for (int i = 0; i < m_WaveData.m_FlyerAmmount[m_CurrentWave]; i++)
            {
                PoolableObject poolableObject = m_Pools[EnemyType.Flyer].GetObject();
                if (currentCount >= WidthLength)
                {
                    currentCount = 0;
                    _Yoffset += 2f;
                    Xoffset = 0f;
                }

                Vector3 _offset = new Vector3(-_Yoffset, 5f, -Xoffset);
                poolableObject.SpawnObject(m_SpawnPoint.position + _offset);
                IEnemy enemy = poolableObject.GetComponent<IEnemy>();
                enemy.Walking(false);
                CurrentAliveEnemys.Add(enemy);
                Xoffset += 2f;
                currentCount++;
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < CurrentAliveEnemys.Count; i++)
            {
                CurrentAliveEnemys[i].Walking(true);
            }
        }
    }

    public int HowManyEnemiesAlive()
    {
        int ammount = 0;
        for (int i = 0; i < CurrentAliveEnemys.Count; i++)
        {
            if (CurrentAliveEnemys[i].IsAlive)
            {
                ammount++;
            }
        }
        return ammount;
    }

    public (int currentWave, int TotalWaves) GetWaveInfo()
    {
        return (m_CurrentWave, m_WaveData.Waves);
    }
}