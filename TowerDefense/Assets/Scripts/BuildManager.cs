using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public enum TowerType
    {
        Archer,
    }

    public static BuildManager Instance;
    private TowerType m_SelectedType;
    private Dictionary<TowerType, ObjectPool> m_Towers;

    [Header("TowerPrefabs")]
    [SerializeField] private GameObject m_ArcherTowerPrefab;

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
        LoadTowers();
    }

    

    private void LoadTowers()
    {
        m_Towers = new Dictionary<TowerType, ObjectPool>();
        ObjectPool _acherTowerPool = gameObject.AddComponent<ObjectPool>();
        _acherTowerPool.BeginPool(m_ArcherTowerPrefab, 5, transform);
        m_Towers.Add(TowerType.Archer, _acherTowerPool);
    }

    public PoolableObject GetSelectedTower()
    {
        return m_Towers[m_SelectedType].GetObject();
    }
}