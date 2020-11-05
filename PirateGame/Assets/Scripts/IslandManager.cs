using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class IslandManager : MonoBehaviour
{
    public static IslandManager Instance;

    private Island[] m_Islands;
    private int m_LastID;

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
        LoadIslands();
    }

    public void RespawnPlayer()
    {
        Player.Instance.transform.position = m_Islands[m_LastID].GetLastLocation();
    }


    public void SetID(int ID)
    {
        m_LastID = ID;
    }

    private void LoadIslands()
    {
        m_Islands = FindObjectsOfType<Island>();
        for (int i = 0; i < m_Islands.Length; i++)
        {
            m_Islands[i].ID = i;
        }
    }
}
