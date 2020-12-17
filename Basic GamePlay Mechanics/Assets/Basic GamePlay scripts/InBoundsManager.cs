using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBoundsManager : MonoBehaviour
{
    public static InBoundsManager Instance;

    [SerializeField] private Vector3 m_MinPos;
    [SerializeField] private Vector3 m_MaxPos;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
    }

    public Vector3 GetRandomPos()
    {
        float X = Random.Range(m_MinPos.x, m_MaxPos.x);
        float Z = Random.Range(m_MinPos.z, m_MaxPos.z);

        return new Vector3(X, 0, Z);
    }

    public bool InBounds(Vector3 vector3)
    {
        if(m_MinPos.x <= vector3.x && m_MaxPos.x > vector3.x)
        {
            if(m_MinPos.z <= vector3.z && m_MaxPos.z >= vector3.z)
            {
                return true;
            }
        }
        return false;
    }
}
