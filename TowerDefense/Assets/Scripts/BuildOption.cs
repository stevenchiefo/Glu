using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildOption : MonoBehaviour
{
    private Tower m_Tower;

    private void OnMouseUpAsButton()
    {
        if (m_Tower == null)
        {
            PoolableObject poolable = BuildManager.Instance.GetSelectedTower();
            m_Tower = poolable.GetComponent<Tower>();
            poolable.SpawnObject(transform.position + new Vector3(-0.3f, 0f, -0.03f));
        }

    }
}