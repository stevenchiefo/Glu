using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Text m_Score;
    [SerializeField] private Image m_HpBar;

    // Update is called once per frame
    private void Update()
    {
        float amount = ShipController.Instance.GetHealth() / ShipController.Instance.m_MaxHealth;
        m_HpBar.fillAmount = amount;

        m_Score.text = $"Score: {GameManager.Instance.GetScore()}";
    }
}