using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Enemy Spawn values")]
    [SerializeField] private Vector3 m_MinPos;
    [SerializeField] private Vector3 m_MaxPos;

    [SerializeField] private GameObject[] m_Animals;
    private ObjectPool[] m_AnimalPools;
    

    [Header("Waves and Enemys Spawn")]
    [SerializeField] private List<int> m_AnimalWaves;
    private List<PoolableObject> m_AliveAnimals;
    private int m_Index;


    private void Start()
    {
        MakePool();
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitUntil(() => CanSpawn());
            if (m_Index < m_AnimalWaves.Count)
            {
                SpawnEnemys(m_AnimalWaves[m_Index]);
                m_Index++;
            }
        }
    }

    private void SpawnEnemys(int ammount)
    {

        for (int i = 0; i < ammount; i++)
        {
            Vector3 pos = GetRandomPos() + transform.position;
            int index = Random.Range(0,m_AnimalPools.Length);
            if (InBoundsManager.Instance.InBounds(pos) == false)
            {
                pos = GetRandomPos() + transform.position;
            }
            PoolableObject poolableObject = m_AnimalPools[index].GetObject();
            poolableObject.SpawnObject(pos);
        }
    }

    

    private bool CanSpawn()
    {
        for (int i = 0; i < m_AliveAnimals.Count; i++)
        {
            if (m_AliveAnimals[i].CanUse() == false)
            {
                m_AliveAnimals.RemoveAt(i);
            }
        }
        return m_AliveAnimals.Count <= 0;
    }

    private void MakePool()
    {
        m_AliveAnimals = new List<PoolableObject>();
        m_AnimalPools = new ObjectPool[m_Animals.Length];
        for (int i = 0; i < m_Animals.Length; i++)
        {
            m_AnimalPools[i] = gameObject.AddComponent<ObjectPool>();
            m_AnimalPools[i].BeginPool(m_Animals[i], 10, transform);
        }
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
        Gizmos.DrawLine(m_MinPos + transform.position, rightmin + transform.position);
        Gizmos.DrawLine(rightmin + transform.position, m_MaxPos + transform.position);
        Gizmos.DrawLine(m_MaxPos + transform.position, rightmax + transform.position);
        Gizmos.DrawLine(rightmax + transform.position, m_MinPos + transform.position);

        
    }



}
