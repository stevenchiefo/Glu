using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : PoolableObject
{
    public int Durrabilty { get; private set; }

    public void Repair(int _amount)
    {
        Durrabilty += _amount;
        if (Durrabilty > 100)
        {
            Durrabilty = 100;
        }
    }

    public override void Load()
    {
        base.Load();
        OnPool.AddListener(() => { StopAllCoroutines(); });
    }

    protected override void ResetObject()
    {
        base.ResetObject();
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        Durrabilty = 100;
        while (Durrabilty > 0)
        {
            yield return new WaitForSeconds(8f);
            Durrabilty -= 8;
        }
        PoolObject();
    }
}