using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
using Unity.Jobs;

public class ObjectPool 
{
    public GameObject m_Prefab;
    public List<PoolableObject> m_Pool;
    public Transform m_Parent;
    public Poolstatistics Statistics;

    public ObjectPool(GameObject _prefab, int _beginLenght, Transform parent)
    {
        m_Prefab = _prefab;
        m_Parent = parent;
        MakePool(_beginLenght, parent);
        
    }

    public void BeginPool(GameObject _prefab, int _beginLenght, Transform parent)
    {
        m_Prefab = _prefab;
        m_Parent = parent;
        MakePool(_beginLenght, parent);
    }

    private void MakePool(int length, Transform parent)
    {
        m_Pool = new List<PoolableObject>();
        for (int i = 0; i < length; i++)
        {
            PoolableObject _poolableObject = null;
            if (parent == null)
            {
                _poolableObject = Object.Instantiate(m_Prefab).GetComponent<PoolableObject>();    //Getcompent poolableobject;
            }
            else
            {
                _poolableObject = Object.Instantiate(m_Prefab, parent).GetComponent<PoolableObject>();
            }
            _poolableObject.Load();
            _poolableObject.HideOjbect();                                                            //Pool the object;
            m_Pool.Add(_poolableObject);                                                              //Add to the list;
        }
    }

    public PoolableObject GetObject()
    {
        for (int i = 0; i < m_Pool.Count; i++)
        {
            if (m_Pool[i].CanUse())
            {
                return m_Pool[i];
            }
        }
        if (m_Parent == null)
        {
            PoolableObject _poolableObject = Object.Instantiate(m_Prefab).GetComponent<PoolableObject>();    //Getcompent poolableobject;
            _poolableObject.Load();
            _poolableObject.PoolObject();                                                             //Pool the object;
            m_Pool.Add(_poolableObject);
            return _poolableObject;
        }
        else
        {
            PoolableObject _poolableObject = Object.Instantiate(m_Prefab, m_Parent).GetComponent<PoolableObject>();    //Getcompent poolableobject;
            _poolableObject.Load();
            _poolableObject.PoolObject();                                                             //Pool the object;
            m_Pool.Add(_poolableObject);
            return _poolableObject;
        }
    }
    /// <summary>
    /// Adjust the pool to the amount given
    /// </summary>
    /// <param name="amount"></param>
    public void AdjustPool(int amount)
    {
        while (m_Pool.Count > amount)
        {
            PoolableObject poolableObject = m_Pool[m_Pool.Count - 1];
            m_Pool.RemoveAt(m_Pool.Count - 1);
            Object.Destroy(poolableObject.gameObject);
        }
    }

    public void SpawnStatistics()
    {
        GameObject obj = Resources.Load<GameObject>("Assets/StevensObjectPool/Prefabs");
        Statistics = Object.Instantiate(obj).GetComponent<Poolstatistics>();
        Statistics.StartCoroutine(Statistics.Timer());
    }
}

public class Poolstatistics : MonoBehaviour
{
    
    public int AmountPerMin;
    public delegate void TaskStatistics();
    public event TaskStatistics OnAdjust;
    
    

    public IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            OnAdjust?.Invoke();
        }
    }
}


