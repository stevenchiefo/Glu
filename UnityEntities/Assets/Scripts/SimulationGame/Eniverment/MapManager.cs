using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public UnityEvent OnBuildHouse;

    [Header("Settings")]
    [SerializeField] private int m_AmountOfStartFurtilzedCells;

    [SerializeField] private GameObject[] m_Prefabs;

    [SerializeField] private GameObject m_HousePrefab;

    private Cell[] m_Cells;

    private Cell[] m_FurtilezedCells;

    private ObjectPool[] m_Pools;

    private Dictionary<ResourceType, ObjectPool> m_DirectPool;

    private ObjectPool m_HousePool;

    public (GrowObject Object, bool AvaiableObject) GetClosestObject(Vector3 _Pos, ResourceType _type, float Range)
    {
        float _closestDistance = 0f;
        GrowObject _bestobj = null;

        foreach (Cell item in m_FurtilezedCells)
        {
            float _currentDistance = Vector3.Distance(item.transform.position, _Pos);
            if (_currentDistance < Range)
            {
                if (item.AlreadyHasAHarvestable)
                {
                    GrowObject _obj = item.GetGrowObject();
                    if (_obj.Type == _type && _obj.IsFurtilzed)
                    {
                        if (_bestobj == null)
                        {
                            _bestobj = _obj;
                            _closestDistance = _currentDistance;
                        }
                        else
                        {
                            if (_closestDistance > _currentDistance)
                            {
                                _bestobj = _obj;
                                _closestDistance = _currentDistance;
                            }
                        }
                    }
                }
            }
        }
        return (_bestobj, _bestobj != null);
    }

    public Cell GetRandomCell()
    {
        return m_Cells[Random.Range(0, m_Cells.Length - 1)];
    }

    public bool BuildHouse(Cell _cell)
    {
        if (!_cell.CanBuild)
            return false;

        _cell.BuildObject(m_HousePool.GetObject());
        OnBuildHouse?.Invoke();
        return true;
    }

    public Cell GetBuildAbleCell(Vector3 _pos, float _range)
    {
        foreach (Cell item in m_Cells)
        {
            float _distance = Vector3.Distance(_pos, item.transform.position);
            if (_distance <= _range)
            {
                if (item.CanBuild && !item.IsFurtile)
                {
                    return item;
                }
            }
        }
        return null;
    }

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

        m_Cells = GetComponentsInChildren<Cell>();
    }

    private void Start()
    {
        m_Pools = new ObjectPool[m_Prefabs.Length];
        m_DirectPool = new Dictionary<ResourceType, ObjectPool>();
        for (int i = 0; i < m_Prefabs.Length; i++)
        {
            m_Pools[i] = gameObject.AddComponent<ObjectPool>();
            m_Pools[i].BeginPool(m_Prefabs[i], 10, transform);

            GrowObject _growable = m_Prefabs[i].GetComponent<GrowObject>();

            if (!m_DirectPool.ContainsKey(_growable.Type))
                m_DirectPool.Add(_growable.Type, m_Pools[i]);
        }

        FurtilizeArea();

        m_HousePool = gameObject.AddComponent<ObjectPool>();
        m_HousePool.BeginPool(m_HousePrefab, 10, transform);
    }

    private void FurtilizeArea()
    {
        List<Cell> _temp = new List<Cell>();
        int alreadyDone = 0;
        while (alreadyDone < m_AmountOfStartFurtilzedCells)
        {
            Cell randomCell = GetRandomCell();
            if (!randomCell.IsFurtile)
            {
                alreadyDone++;
                randomCell.IsFurtile = true;

                PoolableObject _poolObj = m_Pools[Random.Range(0, m_Pools.Length)].GetObject();
                GrowObject _growable = _poolObj.GetComponent<GrowObject>();

                randomCell.AssignResource(_growable.Type);
                randomCell.SpawnObject(_poolObj);

                _temp.Add(randomCell);
            }
        }
        m_FurtilezedCells = _temp.ToArray();
        StartCoroutine(GrowObject());
    }

    private IEnumerator GrowObject()
    {
        while (m_FurtilezedCells.Length > 0)
        {
            yield return new WaitForSeconds(2);
            foreach (Cell item in m_FurtilezedCells)
            {
                if (item.AlreadyHasAHarvestable == false)
                {
                    item.SpawnObject(m_DirectPool[item.Resource].GetObject());
                }
            }
            yield return null;
        }
    }
}