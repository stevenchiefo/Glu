using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : PoolableObject
{
    private TreasueChestData m_TreasueChestData;
    private Rigidbody m_Rigidbody;

    public override void Load()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public override void SpawnObject(Vector3 position, Quaternion rotation)
    {
        base.SpawnObject(position, rotation);
        StartCoroutine(LifetimeTimer());
    }

    private IEnumerator LifetimeTimer()
    {
        yield return new WaitForSeconds(60);
    }

    public void AssignData(TreasueChestData treasueChestData)
    {
        m_TreasueChestData = treasueChestData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckForPlayerShip(collision);
    }

    private void CheckForPlayerShip(Collision collision)
    {
        PlayerShip playerShip = collision.gameObject.GetComponentInParent<PlayerShip>();
        if (playerShip != null)
        {
            Player player = playerShip.GetPlayer();
            player.GiveGold(m_TreasueChestData.Gold);
            player.GiveCannonBalls(m_TreasueChestData.CannonBalls);
            PoolObject();
        }
    }

    public Rigidbody GetRigidbody()
    {
        return m_Rigidbody;
    }
}