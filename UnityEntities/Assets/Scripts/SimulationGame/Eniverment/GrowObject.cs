using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Wood,
    Weat,
}

public class GrowObject : PoolableObject
{
    public ResourceType Type;

    public Vector3 StartSize;
    public Vector3 TargetSize;

    public float GrowSpeed;

    public bool IsFurtilzed;

    public UnityEvent OnHarvest;

    public void Grow()
    {
        StartCoroutine(Scale());
    }

    public ResourceType Harvest()
    {
        OnHarvest?.Invoke();
        OnHarvest.RemoveAllListeners();
        return Type;
    }

    public void Pool()
    {
        PoolObject();
    }

    private IEnumerator Scale()
    {
        IsFurtilzed = false;
        transform.localScale = StartSize;
        float _percent = 0f;

        while (transform.localScale != TargetSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, TargetSize, _percent);
            _percent += GrowSpeed * Time.deltaTime;
            yield return null;
        }

        IsFurtilzed = true;
    }
}