using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class ExpolsionPool : MonoBehaviour
{
    public static ExpolsionPool Instance;

    [SerializeField] private GameObject ExplosionPrefab;
    private ObjectPool m_Pool;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        m_Pool = gameObject.AddComponent<ObjectPool>();
        m_Pool.BeginPool(ExplosionPrefab, 10,transform);
    }
    
    public void SpawnExplosion(Vector2 _pos)
    {
        PoolableObject poolableObject = m_Pool.GetObject();
        poolableObject.SpawnObject(_pos);
    }
}
