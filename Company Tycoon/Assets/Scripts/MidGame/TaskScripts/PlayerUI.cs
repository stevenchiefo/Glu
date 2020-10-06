using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    [SerializeField] private Text m_Currncy;
    [SerializeField] private Text m_CompanyName;
    [SerializeField] private Text m_LevelText;
    [SerializeField] private Image m_Image;

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
        StartCoroutine(DelayUpdate());
    }

    private IEnumerator DelayUpdate()
    {
        yield return new WaitForSeconds(0.5f);
        UpDateUI();
    }

    public void UpDateUI()
    {
        m_Currncy.text = $"{Player.Instance.GetPlayerData().Money.ToString()}$";
        m_CompanyName.text = Player.Instance.GetPlayerData().CompanyName;
        m_LevelText.text = $"Level: {Player.Instance.GetPlayerData().Level}";
        SetBar();
    }

    private void SetBar()
    {
        PlayerStats playerStats = Player.Instance.GetPlayerData();
        float amount = playerStats.XP / playerStats.AmountToLevelUp;
        m_Image.fillAmount = amount;
    }
}