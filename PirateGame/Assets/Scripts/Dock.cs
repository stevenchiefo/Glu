using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour
{
    [SerializeField] private Transform m_DockTransform;
    [SerializeField] private float m_DockingDistance;
    public int ID;

    public void DockShip(PlayerShip playerShip, Player player)
    {
        float _Distance = Vector3.Distance(playerShip.transform.position, m_DockTransform.position);
        if (_Distance <= m_DockingDistance)
        {
            playerShip.transform.position = m_DockTransform.position;
            playerShip.transform.rotation = m_DockTransform.rotation;
            player.LeaveShip(transform.position + new Vector3(0f, 15f, 0f));
        }
    }

    public void RespawnShip(PlayerShip playerShip, Player player)
    {
        playerShip.transform.position = m_DockTransform.position;
        playerShip.transform.rotation = m_DockTransform.rotation;
        player.LeaveShip(transform.position + new Vector3(0f, 15f, 0f));
    }

    
}