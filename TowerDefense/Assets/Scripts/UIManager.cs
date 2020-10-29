using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //Instance
    public static UIManager Instance;

    //Texts && Images

    [SerializeField] private Text m_WavesRemaingText;
    [SerializeField] private Text m_EnemiesRemainingText;
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
    private void Start()
    {
        UpdateUI();
        ShowOrHideLevelComplete(false);
        ShowOrHideLevelFailed(false);
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
        float _Precentage = (float)Finish.Instance.GetInfo().currentHealth / (float)Finish.Instance.GetInfo().MaxHealth;
        string _HealthContext = $"HP: {Finish.Instance.GetInfo().currentHealth}/{Finish.Instance.GetInfo().MaxHealth}";
        m_HealthText.text = _HealthContext;
        m_HealthBar.fillAmount = _Precentage;

        string _EnemysRemaining = $"Enemies Remaining: {EnityManager.Instance.HowManyEnemiesAlive()}";
        string _WaveContext = $"Wave: {EnityManager.Instance.GetWaveInfo().currentWave} / {EnityManager.Instance.GetWaveInfo().TotalWaves}";
        m_WavesRemaingText.text = _WaveContext;
        m_EnemiesRemainingText.text = _EnemysRemaining;
    }
    public void OnRetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnBackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void OnNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}