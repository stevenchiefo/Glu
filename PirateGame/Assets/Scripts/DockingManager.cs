using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingManager : MonoBehaviour
{
    public static DockingManager Instance;

    private Dock[] m_Docks;
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
        LoadDocks();
    }

    public void SetID(int ID)
    {
        m_LastID = ID;
    }

    public void DockOnClosestDock(PlayerShip ship, Player player)
    {
        int _index = 0;
        for (int i = 1; i < m_Docks.Length; i++)
        {
            float _Distance = Vector3.Distance(m_Docks[i].transform.position, ship.transform.position);
            float _ClosestDistance = Vector3.Distance(m_Docks[_index].transform.position, ship.transform.position);
            if (_Distance < _ClosestDistance)
            {
                _index = i;
            }
        }
        m_Docks[_index].DockShip(ship, player);
    }

    public void RespawnBoat(PlayerShip ship, Player player)
    {
        m_Docks[m_LastID].RespawnShip(ship, player);
    }

    private void LoadDocks()
    {
        m_Docks = FindObjectsOfType<Dock>();
        for (int i = 0; i < m_Docks.Length; i++)
        {
            m_Docks[i].ID = i;
        }
    }
}