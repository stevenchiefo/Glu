using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityUIManager : MonoBehaviour
{
    public static EntityUIManager Instance;

    [SerializeField] private TextMeshProUGUI m_TotalVillagers;
    [SerializeField] private TextMeshProUGUI m_NewBornVillagers;
    [SerializeField] private TextMeshProUGUI m_AverageMaxHealth;
    [SerializeField] private TextMeshProUGUI m_AverageSpeed;
    [SerializeField] private TextMeshProUGUI m_AverageDetectioRange;
    [SerializeField] private TextMeshProUGUI m_AverageHungerTreshHold;
    [SerializeField] private TextMeshProUGUI m_AverageSexualTreshHold;
    [SerializeField] private TextMeshProUGUI m_AverageConfidence;
    [SerializeField] private TextMeshProUGUI m_AverageAproachAble;
    [SerializeField] private TextMeshProUGUI m_AverageApeal;
    [SerializeField] private TextMeshProUGUI m_AverageApealTreshHold;
    [SerializeField] private TextMeshProUGUI m_AverageLoyalty;
    [SerializeField] private TextMeshProUGUI m_AverageSparkTreshHold;
    [SerializeField] private TextMeshProUGUI m_AverageHouses;

    public void FirstLoad()
    {
        UpdateTotals();
        UpdateGenes();
        UpdateHouses();
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
    }

    private void Start()
    {
        EnitiyManager.Instance.OnVillagerBorn.AddListener(UpdateGenes);
        EnitiyManager.Instance.OnVillagerBorn.AddListener(UpdateTotals);
        EnitiyManager.Instance.OnVillagerDead.AddListener(UpdateTotals);
        EnitiyManager.Instance.OnVillagerDead.AddListener(UpdateGenes);
        MapManager.Instance.OnBuildHouse.AddListener(UpdateHouses);
    }

    private void UpdateHouses()
    {
        int _totalHouses = 0;
        Villager[] _totalVillagers = EnitiyManager.Instance.TotalVillages;
        foreach (Villager _vil in _totalVillagers)
        {
            if (_vil.VillagerHouse != null)
            {
                _totalHouses++;
            }
        }
        m_AverageHouses.text = $"Villagers with an House:\n{_totalHouses}";
    }

    private void UpdateTotals()
    {
        m_TotalVillagers.text = $"Total Villagers:\n{EnitiyManager.Instance.TotalVillages.Length}";
        m_NewBornVillagers.text = $"Total New Born:\n{EnitiyManager.Instance.AmountOfNewBorn}";
    }

    private void UpdateGenes()
    {
        float _maxHealth = 0f;
        float _speed = 0f;
        float _detectioRange = 0f;
        float _hungerTreshHold = 0f;
        float _sexualTreshHold = 0f;
        float _confidence = 0f;
        float _aproachable = 0f;
        float _apeal = 0f;
        float _apealTreshHold = 0f;
        float _loyalty = 0f;
        float _sparkTreshHold = 0f;

        Villager[] _totalVillagers = EnitiyManager.Instance.TotalVillages;

        foreach (Villager _vil in _totalVillagers)
        {
            _maxHealth += _vil.MaxHealth;
            _speed += _vil.Speed;
            _detectioRange += _vil.DetectionRange;
            _hungerTreshHold += _vil.HungerTreshHold;
            _sexualTreshHold += _vil.SexualTreshHold;
            _confidence += _vil.Confidence;
            _aproachable += _vil.AproachAble;
            _apeal += _vil.Apeal;
            _apealTreshHold += _vil.ApealTreshHold;
            _loyalty += _vil.Loyalty;
            _sparkTreshHold += _vil.SparkTreshHold;
        }

        _maxHealth = _maxHealth / _totalVillagers.Length;
        _speed = _speed / _totalVillagers.Length;
        _detectioRange = _detectioRange / _totalVillagers.Length;
        _hungerTreshHold = _hungerTreshHold / _totalVillagers.Length;
        _sexualTreshHold = _sexualTreshHold / _totalVillagers.Length;
        _confidence = _confidence / _totalVillagers.Length;
        _aproachable = _aproachable / _totalVillagers.Length;
        _apeal = _apeal / _totalVillagers.Length;
        _apealTreshHold = _apealTreshHold / _totalVillagers.Length;
        _loyalty = _loyalty / _totalVillagers.Length;
        _sparkTreshHold = _sparkTreshHold / _totalVillagers.Length;

        m_AverageMaxHealth.text = $"Average Max Health:\n{_maxHealth}";
        m_AverageSpeed.text = $"Average Speed:'\n{_speed}";
        m_AverageDetectioRange.text = $"Average DetectionRange:\n{_detectioRange}";
        m_AverageHungerTreshHold.text = $"Average HungerTreshHold:\n{_hungerTreshHold}";
        m_AverageSexualTreshHold.text = $"Average SexualTreshHold:\n{_sexualTreshHold}";
        m_AverageConfidence.text = $"Average Confidence:\n{_confidence}";
        m_AverageAproachAble.text = $"Average Aproachable:\n{_aproachable}";
        m_AverageApeal.text = $"Average Apeal:\n{_apeal}";
        m_AverageApealTreshHold.text = $"Average Apeal TreshHold:\n{_apealTreshHold}";
        m_AverageLoyalty.text = $"Average loyalty:\n{_loyalty}";
        m_AverageSparkTreshHold.text = $"Average Spark TreshHold:\n{_sparkTreshHold}";
    }
}