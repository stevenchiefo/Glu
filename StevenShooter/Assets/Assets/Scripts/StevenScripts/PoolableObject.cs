using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

public class PoolableObject : MonoBehaviour
{
    private bool m_CanUse;

    protected UnityEvent OnPool = new UnityEvent();

    public void PoolObject()
    {
        gameObject.SetActive(false);
        m_CanUse = true;
    }

    public virtual void SpawnObject(Vector2 position)
    {
        ResetObject();
        transform.position = position;
        gameObject.SetActive(true);
        m_CanUse = false;
    }

    public virtual void SpawnObject(Vector2 position, Quaternion rotation)
    {
        ResetObject();
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
        m_CanUse = false;
    }

    public IEnumerator LifeTimeTimer(int lifetime)
    {
        while (true)
        {
            yield return new WaitForSeconds(lifetime);
            StopCoroutine(LifeTimeTimer(lifetime));
            PoolObject();
        }
    }

    public bool CanUse() => m_CanUse;

    protected virtual void ResetObject()
    {
    }
}