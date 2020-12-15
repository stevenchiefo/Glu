using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorTree : PoolableObject
{
    [SerializeField] private ProcessorOrb[] m_GrowPoints;
    [SerializeField] private int m_GrowSpeed;

    public override void Load()
    {
        OnSpawn.AddListener(StartAllCoroutine);
    }

    private void StartAllCoroutine()
    {
        StartCoroutine(GrowPoints());
    }

    public ProcessorOrb GetClosestOrb(Vector3 vector3)
    {
        int index = 0;
        float lastDistance = Vector3.Distance(vector3, m_GrowPoints[0].transform.position);
        for (int i = 1; i < m_GrowPoints.Length; i++)
        {
            float distance = Vector3.Distance(vector3, m_GrowPoints[i].transform.position);
            if (lastDistance > distance)
            {
                index = i;
                lastDistance = distance;
            }
        }
        return m_GrowPoints[index];
    }

    private IEnumerator GrowPoints()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_GrowSpeed);
            for (int i = 0; i < m_GrowPoints.Length; i++)
            {
                m_GrowPoints[i].SetSize(m_GrowPoints[i].m_Amount + 1);
            }
        }
    }
}