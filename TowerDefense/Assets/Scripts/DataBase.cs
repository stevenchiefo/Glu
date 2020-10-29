using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase Instance;

    [SerializeField] private EnemyData m_RunnerData;
    [SerializeField] private EnemyData m_FlyerData;
    [SerializeField] private TowerData m_ArcherTower;
    [SerializeField] private ShootAbleData m_ArcherArrow;
    

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

    public EnemyData GetEnemyData(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Runner:
                return m_RunnerData;
            case EnemyType.Flyer:
                return m_FlyerData;
        }
        return null;
    }

    public TowerData GetTowerData(TowerType towerType)
    {
        switch (towerType)
        {
            case TowerType.Archer:
                return m_ArcherTower;

        }
        return null;
    }
    public ShootAbleData GetShootAbleData(ShootAbleType shootAbleType)
    {
        switch (shootAbleType)
        {
            case ShootAbleType.ArcherArrow:
                return m_ArcherArrow;
            
        }
        return null;
    }
}
