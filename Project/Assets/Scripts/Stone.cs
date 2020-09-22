using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IDamageble
{
    public int HitPoints;

    public void TakeDamage(int damage)
    {
        HitPoints -= damage;
        if (HitPoints <= 0)
        {
            DestroyStone();
        }
    }

    private void DestroyStone()
    {
        Destroy(gameObject);
    }
}