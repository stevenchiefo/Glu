using Steering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAvoidance : Behavor
{
    public float Radius;
    private LayerMask LayerMask;

    public ObjectAvoidance(float radius, LayerMask layerMask)
    {
        Radius = radius;
        LayerMask = layerMask;
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        if (Priorty == 0)
        {
            return Vector3.zero;
        }
        m_PositionTarget = GetAllDir(behavorContext);
        Vector3 dir = m_PositionTarget - behavorContext.Position;

        float angle = Vector3.Angle(m_VelocityDesired, behavorContext.Velocity);
        if (angle >= 0 && m_PositionTarget != Vector3.zero)
        {
            dir += Vector3.Cross(Vector3.up, behavorContext.Velocity) * behavorContext.Settings.m_MaxVelocityDesired;

        }

        m_VelocityDesired = dir * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }

    public float CaculatePriorty(BehavorContext context)
    {
        float priorty = 0.0f;
        Collider[] Colliders = Physics.OverlapSphere(context.Position, Radius, LayerMask);
        if (Colliders.Length > 0)
        {
            foreach (Collider col in Colliders)
            {
                Vector3 dir = col.transform.position - context.Position;
                if (Physics.Raycast(context.Position, dir, out RaycastHit hit))
                {
                    priorty = GetPriorty(Vector3.Distance(context.Position, hit.point));
                    Priorty = priorty;
                    return priorty;
                }
            }
        }
        Priorty = priorty;
        return priorty;
    }

    private Vector3 GetAllDir(BehavorContext context)
    {
        Collider[] Colliders = Physics.OverlapSphere(context.Position, Radius, LayerMask);
        Vector3 Dir = Vector3.zero;
        foreach (Collider collider in Colliders)
        {
            if (collider.tag.ToLower() != "ground")
            {
                Vector3 directiontoCol = collider.transform.position - context.Position;
                if (Physics.Raycast(context.Position, directiontoCol, out RaycastHit hit))
                {
                    Dir += (((hit.point - collider.transform.position)) * GetMultiPlyer(Vector3.Distance(context.Position, hit.point))) * Radius - context.Velocity;
                }
            }
        }
        return Dir;
    }

    private float GetMultiPlyer(float distance)
    {
        float ammount = 0;
        if (distance < 1)
        {
            ammount = 1 - distance * 1.5f;
            return 2f + ammount;
        }
        ammount = 1 - ((distance * 1.5f) / 10f);
        return 2f + ammount;
    }

    private float GetPriorty(float distance)
    {
        float ammount = 0;
        if (distance <= Radius * 0.95f)
        {
            ammount = 0.7f;
            return ammount;
        }
        ammount = 0.2f;
        return ammount;
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {

        base.OnDrawGizmos(behavorContext);
        Color color = Color.blue;
        Support.DrawWiredSphere(behavorContext.Position, Radius, color);
        if (Priorty > 0)
        {
            color.a = 50f;
            Support.DrawLine(behavorContext.Position, behavorContext.Position + behavorContext.Velocity, Color.white);
            Support.DrawWiredSphere(m_PositionTarget, 1f, Color.red);
        }
    }
}