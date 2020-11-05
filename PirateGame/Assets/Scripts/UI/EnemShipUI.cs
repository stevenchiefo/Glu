using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemShipUI : MonoBehaviour
{
    

    [SerializeField] private GameObject m_UI;
    [SerializeField] private Image m_HealthBar;

    [SerializeField] private TextMeshProUGUI m_HPText;
    private EnemyShip m_EnemyShip;

    private delegate void EnemyShipTask();

    private event EnemyShipTask OnUpdate;

    private Camera m_Camera;



    public void Load()
    {
        m_EnemyShip = GetComponent<EnemyShip>();
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
        float _precentage = (float)m_EnemyShip.Durrability / (float)DataBase.Instance.GetData().EnemyShipData.MaxDurrabilty;
        m_HealthBar.fillAmount = _precentage;
        string _HPContext = $"{DataBase.Instance.GetData().EnemyShipData.MaxDurrabilty}/{m_EnemyShip.Durrability}";
        m_HPText.text = _HPContext;
    }
}
