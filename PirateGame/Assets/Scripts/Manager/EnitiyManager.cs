using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class EnitiyManager : MonoBehaviour
{
    public static EnitiyManager Instance;

    [SerializeField] private List<Transform> m_SpawnPoints;
    [SerializeField] private GameObject m_PirateShipPrefab;
    [SerializeField] private int m_MinWaitSpawnCD;
    [SerializeField] private int m_MaxWaitSpawnCD;
    [SerializeField] private int m_MaxEnemies;
    private ObjectPool m_PirateObjectPool;

    private List<IShip> m_CurrentEnemysAlive;

    //Navegation

    [SerializeField] private List<Transform> m_DestanationPoints;

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
        LoadPool();
        StartCoroutine(CheckForSpawnCooldown());
    }

    private void Update()
    {
        CheckIfAllEnemies();
    }

    private IEnumerator CheckForSpawnCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(m_MinWaitSpawnCD, m_MaxWaitSpawnCD));
            yield return new WaitUntil(() => CanSpawnEnemies());

            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        int _index = Random.Range(0, m_SpawnPoints.Count);
        PoolableObject poolableObject = m_PirateObjectPool.GetObject();
        poolableObject.SpawnObject(m_SpawnPoints[_index].position, m_SpawnPoints[_index].rotation);
        m_CurrentEnemysAlive.Add(poolableObject.GetComponent<IShip>());
    }

    private void LoadPool()
    {
        m_PirateObjectPool = gameObject.AddComponent<ObjectPool>();
        m_PirateObjectPool.BeginPool(m_PirateShipPrefab, 5, this.transform);
        m_CurrentEnemysAlive = new List<IShip>();
    }

    private bool CanSpawnEnemies()
    {
        if (m_CurrentEnemysAlive.Count >= m_MaxEnemies)
        {
            return false;
        }
        int ammount = 0;
        for (int i = 0; i < m_CurrentEnemysAlive.Count; i++)
        {
            if (m_CurrentEnemysAlive[i].Durrability <= 0)
            {
                ammount++;
            }
        }

        return ammount == m_CurrentEnemysAlive.Count;
    }

    private void CheckIfAllEnemies()
    {
        int ammount = 0;
        for (int i = 0; i < m_CurrentEnemysAlive.Count; i++)
        {
            if (m_CurrentEnemysAlive[i].Durrability <= 0)
            {
                ammount++;
            }
        }
        if (ammount == m_CurrentEnemysAlive.Count)
        {
            m_CurrentEnemysAlive.Clear();
        }
    }

    public Vector3 GetNewPoint(Vector3 AlreadyPoint)
    {
        Vector3 Newpos = m_DestanationPoints[Random.Range(0, m_DestanationPoints.Count)].position;
        while (AlreadyPoint == Newpos)
        {
            Newpos = m_DestanationPoints[Random.Range(0, m_DestanationPoints.Count)].position;
        }
        return Newpos;
    }
}