using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;

    [SerializeField] private GameObject m_LootChestPrefab;
    private ObjectPool m_ObjectPool;

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
    }

    public PoolableObject SpawnTreasueChest(Vector3 _Pos, Quaternion _Rot, TreasueChestData treasueChestData)
    {
        PoolableObject poolableObject = m_ObjectPool.GetObject();
        poolableObject.SpawnObject(_Pos, _Rot);
        TreasureChest treasureChest = poolableObject.GetComponent<TreasureChest>();
        treasureChest.AssignData(treasueChestData);

        return poolableObject;
    }

    private void LoadPool()
    {
        m_ObjectPool = gameObject.AddComponent<ObjectPool>();
        m_ObjectPool.BeginPool(m_LootChestPrefab, 10, transform);
    }
}

public struct TreasueChestData
{
    public int Gold;
    public int CannonBalls;
}