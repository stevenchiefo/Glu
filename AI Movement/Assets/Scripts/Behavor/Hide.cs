using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Hide : Behavor
{
    private Collider[] m_HideColliders;
    private Vector3[] m_HideSpots;
    private Transform m_Target;

    public Hide(Transform target)
    {
        m_Target = target;
    }

    public override void Start(BehavorContext behavorContext)
    {
        base.Start(behavorContext);
        m_HideColliders = Physics.OverlapSphere(behavorContext.Position, behavorContext.Settings.m_HideSearchRadius, behavorContext.Settings.m_HideLayerMask);
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        if (m_HideColliders.Length > 0)
        {
            m_PositionTarget = GetClosestPoint(behavorContext);
            m_VelocityDesired = (m_PositionTarget - behavorContext.Position) * behavorContext.Settings.m_MaxVelocityDesired;
            return m_VelocityDesired - behavorContext.Velocity;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        foreach (Vector3 hidespot in m_HideSpots)
        {
            Support.DrawWiredSphere(hidespot, 1, Color.blue);
        }
    }

    private Vector3 GetClosestPoint(BehavorContext context)
    {
        m_HideSpots = GetHideSpots(context);
        float[] values = new float[m_HideSpots.Length];
        for (int i = 0; i < m_HideSpots.Length; i++)
        {
            values[i] = Vector3.Distance(m_Target.position, m_HideSpots[i]);
        }
        return m_HideSpots[GetIndexFromFar(values)];
    }

    private Vector3[] GetHideSpots(BehavorContext context)
    {
        Vector3[] hidespots = new Vector3[m_HideColliders.Length];
        for (int i = 0; i < m_HideColliders.Length; i++)
        {
            Vector3 dir = m_HideColliders[i].transform.position - m_Target.position;
            Vector3 closestpoint = m_HideColliders[i].ClosestPoint(m_HideColliders[i].transform.position + dir);
            hidespots[i] = closestpoint + (dir.normalized * context.Settings.m_HideOffset);
        }
        return hidespots;
    }

    private int GetIndexFromFar(float[] values)
    {
        int index = 0;
        for (int i = 1; i < values.Length; i++)
        {
            if (values[index] < values[i])
            {
                index = i;
            }
        }
        return index;
    }
}