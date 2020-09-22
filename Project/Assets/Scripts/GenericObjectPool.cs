using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool<T> where T : new()
{
    private List<T> NotUsed = new List<T>();
    private List<T> InUse = new List<T>();

    public T Get()
    {
        if (NotUsed.Count != 0)
        {
            T _obj = NotUsed[0];
            InUse.Add(_obj);
            NotUsed.RemoveAt(0);
            return _obj;
        }
        else
        {
            T _obj = new T();
            InUse.Add(_obj);
            return _obj;
        }
    }

    public void Add(T _obj)
    {
        NotUsed.Add(_obj);
    }

    public void ReturnObject(T _obj)
    {
        _obj = CleanUpObj(_obj);
        NotUsed.Add(_obj);
        InUse.Remove(_obj);
    }

    public T CleanUpObj(T _obj)
    {
        _obj = default;
        return _obj;
    }
}