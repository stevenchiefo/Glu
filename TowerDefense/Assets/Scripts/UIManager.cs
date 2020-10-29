using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Instance
    public static UIManager Instance;

    //Texts && Images

    [SerializeField] private Text m_WavesRemaingText;
    [SerializeField] private Image m_HealthBar;
    [SerializeField] private Text m_HealthText;

    //Panels

    [SerializeField] private GameObject m_LevelCompletePanel;
    [SerializeField] private GameObject m_LevelFailedPanel;
    [SerializeField] private GameObject m_GamePlayUIPanel;

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

    public void ShowOrHideLevelComplete(bool boolean)
    {
        m_LevelCompletePanel.SetActive(boolean);
    }

    public void ShowOrHideLevelFailed(bool boolean)
    {
        m_LevelFailedPanel.SetActive(boolean);
    }

    public void ShowOrHideGamePlayerUi(bool boolean)
    {
        m_GamePlayUIPanel.SetActive(boolean);
    }

    public void UpdateUI()
    {
        float _Precentage = Finish.Instance.GetInfo().currentHealth / Finish.Instance.GetInfo().MaxHealth;
        string _HealthContext = $" {Finish.Instance.GetInfo().currentHealth}/{Finish.Instance.GetInfo().MaxHealth}";
        m_HealthText.text = _HealthContext;
        m_HealthBar.fillAmount = _Precentage;

        string _WaveContext = EnityManager;
    }
}