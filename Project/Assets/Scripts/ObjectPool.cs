using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> m_Pool = new List<GameObject>();
    private GameObject m_Prefab;

    public void MakePool(int amount, GameObject _prefab)
    {
        m_Prefab = _prefab;
        for (int i = 0; i < amount; i++)
        {
            GameObject gameObject = Instantiate(_prefab);
            gameObject.SetActive(false);
            m_Pool.Add(gameObject);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < m_Pool.Count; i++)
        {
            if (!m_Pool[i].activeSelf)
            {
                return m_Pool[i];
            }
        }

        GameObject gameObject = Instantiate(m_Prefab);
        gameObject.SetActive(false);
        m_Pool.Add(gameObject);
        return gameObject;
    }
}