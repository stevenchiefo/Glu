using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnitiyManager : MonoBehaviour
{
    public static EnitiyManager Instance;

    public Villager[] TotalVillages { get { return m_AllVillagers.ToArray(); } }

    public int AmountOfNewBorn { get; private set; }

    public UnityEvent OnVillagerBorn;

    public UnityEvent OnVillagerDead;

    [Header("EntitiesSettings")]
    [SerializeField] private EntitySettings m_Settings;

    [Header("Villager Settings")]
    [SerializeField] private int m_StartAmountOfMales;

    [SerializeField] private GameObject m_MalePrefab;

    [SerializeField] private int m_StartAmountOfFemales;
    [SerializeField] private GameObject m_FemalePrefab;

    [Header("QuadTree Settings")]
    [SerializeField] private int Size;

    [SerializeField] private int Capicity;

    private QuadTree<Villager> m_Units;

    private Dictionary<Sex, ObjectPool> m_VillagerPools;

    private List<Villager> m_AllVillagers;

    public void AddVillager(Villager _villager)
    {
        if (!m_AllVillagers.Contains(_villager))
        {
            m_AllVillagers.Add(_villager);
            OnVillagerBorn?.Invoke();
        }
    }

    public void RemoveVillager(Villager _villager)
    {
        if (m_AllVillagers.Contains(_villager))
        {
            m_AllVillagers.Remove(_villager);
            OnVillagerDead?.Invoke();
        }
    }

    public void CreateVillager(VillagerGenes _genes)
    {
        AmountOfNewBorn++;
        SpawnVillager(_genes);
    }

    public Villager[] GetVillagersInRange(Vector3 _pos, float _range)
    {
        return m_Units.GetPoints(_pos, _range).ToArray();
    }

    public VillagerGenes GetRandomGene(Sex _gender)
    {
        return new VillagerGenes
        {
            Age = Random.Range(20, 40),
            MaxHealth = Random.Range(m_Settings.MinMaxHealth, m_Settings.MaxMaxHealth),
            DetectionRange = Random.Range(m_Settings.MinDetectionRange, m_Settings.MaxDetectionRange),
            HungerTreshHold = Random.Range(m_Settings.MinHungerTreshHold, m_Settings.MaxHungerTreshHold),
            Speed = Random.Range(m_Settings.MinSpeed, m_Settings.MaxSpeed),
            SexualTreshHold = Random.Range(m_Settings.MinSexualTreshHold, m_Settings.MaxSexualTreshHold),
            Loyalty = Random.Range(m_Settings.MinLoyalty, m_Settings.MaxLoyalty),
            SparkTreshHold = Random.Range(m_Settings.MinSparkTreshHold, m_Settings.MaxSprakTreshHold),
            AproachAble = Random.Range(m_Settings.MinAproachable, m_Settings.MaxAproachable),
            Apeal = Random.Range(m_Settings.MinApeal, m_Settings.MaxApeal),
            ApealTreshHold = Random.Range(m_Settings.MinApealTreshHold, m_Settings.MaxApealTreshHold),
            Confidence = Random.Range(m_Settings.MinConfidence, m_Settings.MaxConfidence),
            Gender = _gender,
        };
    }

    public VillagerGenes MixGenes(Villager _Dad, Villager _Mother)
    {
        return new VillagerGenes
        {
            Age = 0,
            MaxHealth = GetRandomLerpValue(_Dad.MaxHealth, _Mother.MaxHealth),
            DetectionRange = GetRandomLerpValue(_Dad.MaxHealth, _Mother.MaxHealth),
            HungerTreshHold = GetRandomLerpValue(_Dad.HungerTreshHold, _Mother.HungerTreshHold),
            Speed = GetRandomLerpValue(_Dad.Speed, _Mother.Speed),
            SexualTreshHold = GetRandomLerpValue(_Dad.SexualTreshHold, _Mother.SexualTreshHold),
            Loyalty = GetRandomLerpValue(_Dad.Loyalty, _Mother.Loyalty),
            SparkTreshHold = GetRandomLerpValue(_Dad.SparkTreshHold, _Mother.SparkTreshHold),
            AproachAble = GetRandomLerpValue(_Dad.AproachAble, _Mother.AproachAble),
            Apeal = GetRandomLerpValue(_Dad.Apeal, _Mother.Apeal),
            ApealTreshHold = GetRandomLerpValue(_Dad.ApealTreshHold, _Mother.ApealTreshHold),
            Confidence = GetRandomLerpValue(_Dad.Confidence, _Mother.Confidence),
            Gender = GetRandomGender(),
        };
    }

    public Sex GetRandomGender()
    {
        int _amount = Random.Range(-1, 1);
        if (_amount < 0)
            return Sex.Male;
        else
            return Sex.Female;
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
        m_AllVillagers = new List<Villager>();
        CreatePools();
    }

    private void Start()
    {
        m_Units = new QuadTree<Villager>(Size, Capicity, transform.position);
        SpawnStartAmount();
    }

    private void Update()
    {
        m_Units.SpawnPoints(m_AllVillagers);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_Units != null)
        {
            m_Units.OnDrawGizmos();
        }
    }

    private void SpawnVillager(VillagerGenes _genes)
    {
        PoolableObject _object = null;
        switch (_genes.Gender)
        {
            case Sex.Male:
                _object = m_VillagerPools[Sex.Male].GetObject();
                break;

            case Sex.Female:
                _object = m_VillagerPools[Sex.Female].GetObject();
                break;
        }

        if (_object == null)
            return;
        Cell _cell = MapManager.Instance.GetRandomCell();

        Vector3 _Placepos = new Vector3(_cell.WorldPosition.x, 1.5f, _cell.WorldPosition.z);
        _object.SpawnObject(_Placepos);

        Villager _villager = _object.GetComponent<Villager>();
        _villager.AssignGenes(_genes);

        m_AllVillagers.Add(_villager);
    }

    private void SpawnStartAmount()
    {
        for (int i = 0; i < m_StartAmountOfMales; i++)
        {
            SpawnVillager(GetRandomGene(Sex.Male));
        }

        for (int i = 0; i < m_StartAmountOfFemales; i++)
        {
            SpawnVillager(GetRandomGene(Sex.Female));
        }
        AmountOfNewBorn = 0;
        EntityUIManager.Instance.FirstLoad();
    }

    private void CreatePools()
    {
        m_VillagerPools = new Dictionary<Sex, ObjectPool>();

        ObjectPool _pool = gameObject.AddComponent<ObjectPool>();
        _pool.BeginPool(m_MalePrefab, 10, null);
        m_VillagerPools.Add(Sex.Male, _pool);

        _pool = gameObject.AddComponent<ObjectPool>();
        _pool.BeginPool(m_FemalePrefab, 10, null);
        m_VillagerPools.Add(Sex.Female, _pool);
    }

    private float GetRandomLerpValue(float _a, float _b)
    {
        return Mathf.Lerp(_a, _b, Random.Range(0f, 1f));
    }
}

public struct VillagerGenes
{
    public int Age;
    public float MaxHealth;
    public float DetectionRange;
    public float HungerTreshHold;
    public float Speed;
    public float SexualTreshHold;
    public float Loyalty;
    public float SparkTreshHold;
    public float Apeal;
    public float ApealTreshHold;
    public float AproachAble;
    public float Confidence;
    public Sex Gender;
}