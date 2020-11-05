using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShipUI : MonoBehaviour
{
    public static PlayerShipUI Instance;

    [SerializeField] private GameObject m_UI;
    [SerializeField] private Image m_HealthBar;

    [SerializeField] private TextMeshProUGUI m_HPText;

    private delegate void PlayerShipTask();

    private event PlayerShipTask OnUpdate;

    private Camera m_Camera;

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
        m_Camera = Camera.main;
        OnUpdate += UpdateHealth;
    }

    private void Update()
    {
        m_UI.transform.LookAt(m_Camera.transform);
    }

    public void UpdateUI()
    {
        OnUpdate();
    }

    private void UpdateHealth()
    {
        float _precentage = (float)PlayerShip.Instance.Durrability / (float)DataBase.Instance.GetData().ShipData.MaxDurrabilty;
        m_HealthBar.fillAmount = _precentage;
        string _HPContext = $"{DataBase.Instance.GetData().ShipData.MaxDurrabilty}/{PlayerShip.Instance.Durrability}";
        m_HPText.text = _HPContext;
    }
}