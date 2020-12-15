using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

public class PoolableObject : MonoBehaviour
{
    private bool m_CanUse;

    public UnityEvent OnPool = new UnityEvent();
    public UnityEvent OnSpawn = new UnityEvent();

    public virtual void Load()
    {
    }

    public void HideOjbect()
    {
        gameObject.SetActive(false);
        m_CanUse = true;
    }

    public void PoolObject()
    {
        OnPool.Invoke();
        gameObject.SetActive(false);
        m_CanUse = true;
    }

    public virtual void SpawnObject(Vector3 position)
    {
        ResetObject();
        transform.position = position;
        gameObject.SetActive(true);
        m_CanUse = false;
        OnSpawn.Invoke();
    }

    public virtual void SpawnObject(Vector3 position, Quaternion rotation)
    {
        ResetObject();
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
        m_CanUse = false;
        OnSpawn.Invoke();
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

    public void LookAtTraget(Transform target)
    {
        transform.LookAt(target);
    }

    public bool CanUse() => m_CanUse;

    protected virtual void ResetObject()
    {
    }
}