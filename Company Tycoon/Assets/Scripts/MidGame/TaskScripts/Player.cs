using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private bool m_Load;

    private delegate void PlayerTask();

    private string m_FilePath = "Playerstats.json";
    private FileOperator m_FileOperator;
    private PlayerStats m_PlayerStats;

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

    public void SetStart(string name)
    {
        m_FileOperator = new FileOperator(m_FilePath);
        if (m_FileOperator.AlreadyExits() && m_Load == true)
        {
            m_PlayerStats = m_FileOperator.ReadFile<PlayerStats>();
        }
        else
        {
            m_PlayerStats = new PlayerStats
            {
                CompanyName = name,
                Level = 0,
                Money = 1000,
                XP = 0,
                AmountToLevelUp = 10,
            };
        }
        PlayerUI.Instance.UpDateUI();
    }

    public void AddXp(float amount)
    {
        float extraamount = m_PlayerStats.XP + amount;
        extraamount = extraamount - m_PlayerStats.AmountToLevelUp;
        m_PlayerStats.XP += amount;

        if (m_PlayerStats.XP >= m_PlayerStats.AmountToLevelUp)
        {
            m_PlayerStats.Level++;
            m_PlayerStats.AmountToLevelUp = m_PlayerStats.AmountToLevelUp * 2;
            m_PlayerStats.XP = 0;
        }

        if (extraamount > m_PlayerStats.AmountToLevelUp)
        {
            AddXp(extraamount);
        }
        PlayerUI.Instance.UpDateUI();
    }

    public void AddMoney(int amount)
    {
        m_PlayerStats.Money = m_PlayerStats.Money + amount;
        PlayerUI.Instance.UpDateUI();
    }

    public void RemoveMoney(int amount)
    {
        m_PlayerStats.Money = m_PlayerStats.Money - amount;
        PlayerUI.Instance.UpDateUI();
    }

    public int GetMoney()
    {
        return m_PlayerStats.Money;
    }

    public PlayerStats GetPlayerData()
    {
        return m_PlayerStats;
    }

    private void OnApplicationQuit()
    {
        StartSave();
    }

    public void StartSave()
    {
        PlayerTask playerTask = new PlayerTask(SaveFile);
        IAsyncResult asyncResult = playerTask.BeginInvoke(null, null);
    }

    private void SaveFile()
    {
        m_FileOperator.WriteFile(m_PlayerStats);
    }
}

[Serializable]
public struct PlayerStats
{
    public string CompanyName;
    public int Money;
    public int Level;
    public float XP;
    public float AmountToLevelUp;
}