using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInterfaceUI : MonoBehaviour
{
    public static PlayerInterfaceUI Instance;

    [SerializeField] private Image m_HealthBar;

    [SerializeField] private TextMeshProUGUI m_HPText;
    [SerializeField] private TextMeshProUGUI m_GoldText;
    [SerializeField] private TextMeshProUGUI m_CannonBallText;

    [SerializeField] private GameObject m_CanDockOBJ;

    private delegate void PlayerShipTask();

    private event PlayerShipTask OnUpdate;

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
        OnUpdate += UpdateHealth;
        OnUpdate += UpdateGold;
        OnUpdate += UpdateCannonBall;
    }

    public void UpdateUI()
    {
        OnUpdate();
    }

    private void UpdateHealth()
    {
        float _precentage = (float)Player.Instance.GetPlayerStats().Health / (float)Player.Instance.GetMaxHealth();
        m_HealthBar.fillAmount = _precentage;
        string _HPContext = $"{Player.Instance.GetMaxHealth()}/{Player.Instance.GetPlayerStats().Health}";
        m_HPText.text = _HPContext;
    }

    private void UpdateGold()
    {
        string _GoldContext = $"{Player.Instance.GetPlayerStats().Gold}";
        m_GoldText.text = _GoldContext;
    }

    private void UpdateCannonBall()
    {
        string _CannonBallText = $"{Player.Instance.GetPlayerStats().CannonBalls}";
        m_CannonBallText.text = _CannonBallText;
    }

    public void SetCanDock(bool _boolean)
    {
        m_CanDockOBJ.SetActive(_boolean);
    }
}