using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour
{
    [SerializeField] private Transform m_DockTransform;
    [SerializeField] private float m_DockingDistance;
    public int ID;
    public bool CanDock;

    private void Update()
    {
        if (PlayerShip.Instance.GetPlayer().IsOnShip())
        {
            float _Distance = Vector3.Distance(PlayerShip.Instance.transform.position, m_DockTransform.position);
            CanDock = _Distance <= m_DockingDistance;
        }
        else
        {
            CanDock = false;
        }
    }

    public void DockShip(PlayerShip playerShip, Player player)
    {
        float _Distance = Vector3.Distance(playerShip.transform.position, m_DockTransform.position);
        if (_Distance <= m_DockingDistance)
        {
            playerShip.transform.position = new Vector3(m_DockTransform.position.x, 0f, m_DockTransform.position.z);
            playerShip.transform.rotation = m_DockTransform.rotation;
            player.LeaveShip(transform.position + new Vector3(0f, 15f, 0f));
        }
    }

    public void RespawnShip(PlayerShip playerShip)
    {
        playerShip.transform.position = m_DockTransform.position;
        playerShip.transform.rotation = m_DockTransform.rotation;
    }

    public void RespawnPlayer(Player player)
    {
        player.LeaveShip(transform.position + new Vector3(0f, 15f, 0f));
    }
}