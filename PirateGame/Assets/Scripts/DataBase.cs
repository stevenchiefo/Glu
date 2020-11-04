using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase Instance;

    [SerializeField] private ShipData m_ShipData;
    [SerializeField] private ShipData m_EnemyShipData;

    [SerializeField] private GameObject m_CanonBallPrefab;
    [SerializeField] private CannonBallData m_CannonBallData;


    private ObjectPool m_CanonBallPool;

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

    private void LoadPools()
    {
        m_CanonBallPool = gameObject.AddComponent<ObjectPool>();
        m_CanonBallPool.BeginPool(m_CanonBallPrefab, 10, transform);
    }

    public (CannonBallData CannonBallData, ShipData ShipData, ShipData EnemyShipData) GetData()
    {
        return (m_CannonBallData, m_ShipData,m_EnemyShipData);
    }

    public PoolableObject GetCannonBall()
    {
        return m_CanonBallPool.GetObject();
    }
}
