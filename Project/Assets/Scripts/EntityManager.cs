using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject tankBuster;
    [SerializeField] private float startEnemySpawnTime;
    [SerializeField] private float minimumEnemySpawnTime;
    [SerializeField] private float spawnTimeDecrease; // Na iedere spawn gaat er zoveel tijd van af
    [SerializeField] private int enemyLimit;
    [SerializeField] private float m_BusterRespawnTime;
    private int m_AmountToKill;
    [HideInInspector] public int currentEnemiesActive;
    private float currentEnemySpawnTime;
    private float enemySpawnTimer;

    private List<GameObject> CurrentTanks;
    private ObjectPool m_EnemyTankPool;
    private ObjectPool m_TankBusterPool;

    public Player Player { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        CurrentTanks = new List<GameObject>();
        currentEnemySpawnTime = startEnemySpawnTime;
        enemySpawnTimer = currentEnemySpawnTime;

        Player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();

        gameObject.AddComponent<ObjectPool>();
        gameObject.AddComponent<ObjectPool>();

        ObjectPool[] objectPools = GetComponents<ObjectPool>();
        m_EnemyTankPool = objectPools[0];
        m_TankBusterPool = objectPools[1];
        m_EnemyTankPool.MakePool(10, enemyPrefab);
        m_TankBusterPool.MakePool(3, tankBuster);

        StartCoroutine(EnemySpawnTimer());
        StartCoroutine(CheckForSpawnBuster());
    }

    private IEnumerator CheckForSpawnBuster()
    {
        while (true)
        {
            if (m_AmountToKill > currentEnemiesActive)
            {
                SpawnBuster();
            }
            yield return new WaitForSeconds(m_BusterRespawnTime);
        }
    }

    private IEnumerator EnemySpawnTimer()
    {
        while (true)
        {
            if (CanSpawnEnemys())
            {
                SpawnEnemys();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private bool CanSpawnEnemys()
    {
        if (currentEnemiesActive == 0)
        {
            return true;
        }
        return false;
    }

    public void SpawnEnemys()
    {
        CurrentTanks.Clear();
        int amount = UnityEngine.Random.Range(3, enemyLimit / 2);
        for (int i = 0; i < amount; i++)
        {
            Transform randomWaypoint = WaypointManager.instance.GetRandomWaypoint();
            Vector3 direction = -randomWaypoint.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            EnemyTank newEnemy = m_EnemyTankPool.GetObject().GetComponent<EnemyTank>();
            newEnemy.SpawnObject(randomWaypoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
            newEnemy.transform.Translate(Vector3.left * 5f, Space.Self);
            CurrentTanks.Add(newEnemy.gameObject);
        }
        currentEnemiesActive = amount;
        m_AmountToKill = UnityEngine.Random.Range(1, amount);
    }

    private void SpawnBuster()
    {
        if (DoSpawnBuster(25))
        {
            Transform randomWaypoint = WaypointManager.instance.GetRandomWaypoint();
            Vector3 direction = -randomWaypoint.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            EnemyBuster newEnemy = m_TankBusterPool.GetObject().GetComponent<EnemyBuster>();
            newEnemy.SpawnObject(randomWaypoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
            CurrentTanks.Add(newEnemy.gameObject);
        }
    }

    private bool DoSpawnBuster(int precent)
    {
        int Procent = UnityEngine.Random.Range(0, 100);
        if (precent < Procent)
        {
            return true;
        }
        return false;
    }

    public Transform GetClosedEnemy(Vector3 pos)
    {
        GameObject closeEnemy = null;
        for (int i = 0; i < CurrentTanks.Count; i++)
        {
            if (CurrentTanks[i].activeSelf)
            {
                if (closeEnemy == null)
                {
                    closeEnemy = CurrentTanks[i];
                }
                else
                {
                    float _Dist1 = Vector3.Distance(closeEnemy.transform.position, pos);
                    float _Dist2 = Vector3.Distance(CurrentTanks[i].transform.position, pos);
                    if (_Dist1 < _Dist2)
                    {
                        closeEnemy = CurrentTanks[i];
                    }
                }
            }
        }
        return closeEnemy.transform;
    }

    public void SetPlayerDead()
    {
    }
}