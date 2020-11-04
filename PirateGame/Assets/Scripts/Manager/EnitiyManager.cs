using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class EnitiyManager : MonoBehaviour
{
    [SerializeField] private List<Transform> m_SpawnPoints;
    [SerializeField] private GameObject m_PirateShipPrefab;
    [SerializeField] private int m_MinWaitSpawnCD;
    [SerializeField] private int m_MaxWaitSpawnCD;
    private ObjectPool m_PirateObjectPool;

    private List<IShip> m_CurrentEnemysAlive;

    void Start()
    {
        LoadPool();
        StartCoroutine(CheckForSpawnCooldown());
        
    }

    

    private IEnumerator CheckForSpawnCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(m_MinWaitSpawnCD, m_MaxWaitSpawnCD));
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        m_CurrentEnemysAlive.Clear();
        int _index = Random.Range(0, m_SpawnPoints.Count);
        PoolableObject poolableObject = m_PirateObjectPool.GetObject();
        poolableObject.SpawnObject(m_SpawnPoints[_index].position, m_SpawnPoints[_index].rotation);
        
    }

    private void LoadPool()
    {
        m_PirateObjectPool = gameObject.AddComponent<ObjectPool>();
        m_PirateObjectPool.BeginPool(m_PirateShipPrefab, 5, this.transform);
        m_CurrentEnemysAlive = new List<IShip>();
    }

}
