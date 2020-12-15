using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class EnitiyManager : MonoBehaviour
{
    public static EnitiyManager instance;

    [SerializeField] private GameObject m_DataCubePrefab;
    [SerializeField, Range(2, 100)] private int m_DataCubesToSpawn;
    public DataCubeSettings DataCubeSettings;

    [SerializeField] private GameObject m_ViriusPrefab;
    [SerializeField, Range(2, 60)] private int m_ViriusToSpawn;
    public ViriusSettings ViriusSettings;

    [SerializeField] private GameObject m_GarbargeCollecterPrefab;
    [SerializeField, Range(2, 10)] private int m_CarbargeCollectersToSpawn;
    public GarbargeCollectorSettings GarbargeCollectorSettings;

    [SerializeField] private GameObject m_MemorySlotPrefab;
    [SerializeField] private GameObject m_ProcessorTreePrefab;
    public Vector3 m_MinPos;
    public Vector3 m_MaxPos;

    private ObjectPool m_DataCubePool;
    private ObjectPool m_ViriusPool;
    private ObjectPool m_GarbargeCollectorPool;
    private ObjectPool m_MemorySlotPool;
    private ObjectPool m_ProcessorTreePrefabPool;

    [Header("MemorySlots")]
    [SerializeField] private int m_MaxMemorySlots;

    private int m_CurrentMemorySlotsAlive;

    [Header("MB and Cpu settings")]
    public MbCpuSettings MbCpuSettings;

    [Header("UI")]
    [SerializeField] private UI m_UI;

    private List<DataCubeBrain> m_AliveDataCubes;
    public int AliveDataCubes;
    public int DataCubesBorn;
    public int DeadDataCubes;

    private List<ViriusBrain> m_AliveVirius;
    public int AliveVirus;
    public int VirusBorn;
    public int DeadVirus;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        StartPools();
    }

    public void CalculateAliveDC()
    {
        for (int i = 0; i < m_AliveDataCubes.Count; i++)
        {
            if (m_AliveDataCubes[i].Dead)
            {
                DeadDataCubes++;
                m_AliveDataCubes.RemoveAt(i);
            }
        }
        AliveDataCubes = m_AliveDataCubes.Count;
        m_UI.UpdateUi();
    }

    public void CalculateAliveV()
    {
        for (int i = 0; i < m_AliveVirius.Count; i++)
        {
            if (m_AliveVirius[i].Dead)
            {
                DeadVirus++;
                m_AliveVirius.RemoveAt(i);
            }
        }
        AliveVirus = m_AliveVirius.Count;
        m_UI.UpdateUi();
    }

    private void Update()
    {
        CalculateAliveDC();
        CalculateAliveV();
    }

    private void StartPools()
    {
        m_DataCubePool = gameObject.AddComponent<ObjectPool>();
        m_DataCubePool.BeginPool(m_DataCubePrefab, m_DataCubesToSpawn, null);

        m_ViriusPool = gameObject.AddComponent<ObjectPool>();
        m_ViriusPool.BeginPool(m_ViriusPrefab, m_ViriusToSpawn, transform);

        m_MemorySlotPool = gameObject.AddComponent<ObjectPool>();
        m_MemorySlotPool.BeginPool(m_MemorySlotPrefab, 10, transform);

        m_ProcessorTreePrefabPool = gameObject.AddComponent<ObjectPool>();
        m_ProcessorTreePrefabPool.BeginPool(m_ProcessorTreePrefab, 10, null);

        m_GarbargeCollectorPool = gameObject.AddComponent<ObjectPool>();
        m_GarbargeCollectorPool.BeginPool(m_GarbargeCollecterPrefab, m_CarbargeCollectersToSpawn, transform);

        m_AliveDataCubes = new List<DataCubeBrain>();
        m_AliveVirius = new List<ViriusBrain>();

        for (int i = 0; i < m_DataCubesToSpawn; i++)
        {
            PoolableObject _obj = m_DataCubePool.GetObject();
            _obj.SpawnObject(GetRandomPos());
            DataCubeBrain dataBrain = _obj.GetComponent<DataCubeBrain>();
            dataBrain.SetRandomStats();
            m_AliveDataCubes.Add(dataBrain);
        }
        CalculateAliveDC();

        for (int i = 0; i < m_ViriusToSpawn; i++)
        {
            PoolableObject _obj = m_ViriusPool.GetObject();
            _obj.SpawnObject(GetRandomPos());
            ViriusBrain virusbrain = _obj.GetComponent<ViriusBrain>();
            virusbrain.SetRandomStats();
            m_AliveVirius.Add(virusbrain);
        }

        for (int i = 0; i < m_CarbargeCollectersToSpawn; i++)
        {
            PoolableObject _obj = m_GarbargeCollectorPool.GetObject();
            _obj.SpawnObject(GetRandomPos());
        }
        int ammount = Mathf.RoundToInt(m_DataCubesToSpawn * 0.50f);
        for (int i = 0; i < ammount; i++)
        {
            PoolableObject _obj = m_ProcessorTreePrefabPool.GetObject();
            _obj.SpawnObject(GetRandomPos());
        }

        StartCoroutine(SpawnMemorySlot());
    }

    private IEnumerator SpawnMemorySlot()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_MaxMemorySlots > m_CurrentMemorySlotsAlive);
            yield return new WaitForSeconds(10);
            int ammount = Random.Range(2, Mathf.RoundToInt(m_DataCubesToSpawn * 0.2f));
            for (int i = 0; i < ammount; i++)
            {
                if (m_CurrentMemorySlotsAlive < m_MaxMemorySlots)
                {
                    PoolableObject poolableObject = m_MemorySlotPool.GetObject();
                    poolableObject.SpawnObject(GetRandomPos());
                    MemorySlot memorySlot = poolableObject.GetComponent<MemorySlot>();
                    memorySlot.SetMB(Random.Range(5, 10));
                    m_CurrentMemorySlotsAlive++;
                }
            }
        }
    }

    public void RemoveMemorySlot()
    {
        m_CurrentMemorySlotsAlive--;
        m_UI.UpdateUi();
    }

    public void MakeaDataCube(DataCubeBrain[] dataCubebrains)
    {
        DataCubesBorn += 1;
        PoolableObject poolableObject = m_DataCubePool.GetObject();
        poolableObject.SpawnObject(dataCubebrains[0].transform.position);
        DataCubeBrain dataCubebrain = poolableObject.GetComponent<DataCubeBrain>();
        dataCubebrain.Born(dataCubebrains);
        m_AliveDataCubes.Add(dataCubebrain);
    }

    public void MakeVirius(ViriusBrain[] _virusBrains)
    {
        VirusBorn += 1;
        PoolableObject poolableObject = m_ViriusPool.GetObject();
        poolableObject.SpawnObject(_virusBrains[0].transform.position);
        ViriusBrain _virus = poolableObject.GetComponent<ViriusBrain>();
        _virus.Born(_virusBrains);
        m_AliveVirius.Add(_virus);
    }

    private Vector2 GetRandomPos()
    {
        float X = Random.Range(m_MinPos.x, m_MaxPos.x);
        float Y = Random.Range(m_MinPos.y, m_MaxPos.y);
        return new Vector2(X, Y);
    }
}