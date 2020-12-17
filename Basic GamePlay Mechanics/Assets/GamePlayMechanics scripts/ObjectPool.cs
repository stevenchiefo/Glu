using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject m_Prefab;
    private List<PoolableObject> m_Pool;
    
    public void BeginPool(GameObject _prefab,int _beginLenght , Transform parent)
    {
        m_Prefab = _prefab;
        MakePool(_beginLenght,parent);
    }

    private void MakePool(int length,Transform parent)
    {
        m_Pool = new List<PoolableObject>();
        for (int i = 0; i < length; i++)
        {
            PoolableObject _poolableObject = null;
            if (parent == null)
            {
               _poolableObject  = Instantiate(m_Prefab).GetComponent<PoolableObject>();    //Getcompent poolableobject;
            }
            else
            {
                _poolableObject = Instantiate(m_Prefab,parent).GetComponent<PoolableObject>();
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
        PoolableObject _poolableObject = Instantiate(m_Prefab).GetComponent<PoolableObject>();    //Getcompent poolableobject;
        _poolableObject.Load();
        _poolableObject.PoolObject();                                                             //Pool the object;
        m_Pool.Add(_poolableObject);
        return _poolableObject;

    }
}
