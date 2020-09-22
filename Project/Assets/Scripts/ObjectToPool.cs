using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToPool : MonoBehaviour
{
    public virtual void PoolObject()
    {
        ResetObject();
        gameObject.SetActive(false);
    }

    public virtual void SpawnObject()
    {
        gameObject.SetActive(true);
    }

    public virtual void SpawnObject(Vector3 vector3)
    {
        gameObject.SetActive(true);
        transform.position = vector3;
    }

    public virtual void SpawnObject(Vector3 vector3, Quaternion quaternion)
    {
        gameObject.SetActive(true);
        transform.position = vector3;
        transform.rotation = quaternion;
    }

    public virtual void ResetObject()
    {
    }
}