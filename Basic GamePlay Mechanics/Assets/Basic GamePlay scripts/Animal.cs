using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : PoolableObject
{
    private void Update()
    {
        CheckForOutOfBounds();
    }

    private void CheckForOutOfBounds()
    {
        if (InBoundsManager.Instance.InBounds(transform.position) == false)
        {
            PoolObject();
        }
    }

    public void Hit()
    {
        PoolObject();
    }
}
