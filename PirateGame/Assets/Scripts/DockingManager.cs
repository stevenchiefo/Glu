using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingManager : MonoBehaviour
{
    public static DockingManager Instance;

    private Dock[] m_Docks;

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

    private void LoadDocks()
    {
        m_Docks = FindObjectsOfType<Dock>();
    }
}