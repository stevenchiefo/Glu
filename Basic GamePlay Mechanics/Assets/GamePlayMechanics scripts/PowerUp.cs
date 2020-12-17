using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PoolableObject
{
    public float Duration;
    public bool PickedUp;

    private void OnTriggerEnter(Collider collider)
    {
        if (CheckForPlayer(collider))
        {
            OnHit(collider.GetComponentInParent<Player>());
            PickedUp = true;
            PoolObject();
        }
    }

    private bool CheckForPlayer(Collider collider)
    {
        Player player = collider.GetComponentInParent<Player>();
        return player != null;
    }

    protected virtual void OnHit(Player player)
    {
    }

    protected override void ResetObject()
    {
        PickedUp = false;
    }
}