using Steering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAvoidance : Behavor
{
    public float Radius;
    private LayerMask LayerMask;

    //GizmosDraw
    private Vector3 m_TargetPos;

    private Vector3 m_LastHitpos;

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


        Vector3 dir = GetAllDir(behavorContext);
        m_PositionTarget = dir + behavorContext.Position;
        
        float angle = Vector3.Angle(m_VelocityDesired, behavorContext.Velocity);
        if (angle >= 0 && angle < 1 && m_PositionTarget != Vector3.zero)
        {
            m_PositionTarget += (Vector3.Cross(behavorContext.Position + Vector3.up, behavorContext.Velocity) * behavorContext.Settings.m_MaxVelocityDesired) * (Radius * 10);
        }
        
        
        m_PositionTarget.y = 0;
        Debug.DrawLine(behavorContext.Position, behavorContext.Position + dir);

        m_VelocityDesired = (m_PositionTarget - behavorContext.Position) * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }

    public float CaculatePriorty(BehavorContext context)
    {
        float priorty = 0.0f;
        Collider[] Colliders = Physics.OverlapSphere(context.Position, Radius * 1.2f, LayerMask);
        if (Colliders.Length > 0)
        {
            foreach (Collider col in Colliders)
            {
                Vector3 dir = col.transform.position - context.Position;
                if (Physics.Raycast(context.Position, dir, out RaycastHit hit))
                {
                    priorty = GetPriorty(Vector3.Distance(context.Position, hit.point), context);
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
        Collider[] Colliders = Physics.OverlapSphere(context.Position, Radius * 1.2f, LayerMask);
        Vector3 Dir = Vector3.zero;
        foreach (Collider collider in Colliders)
        {
            Vector3 directiontoCol = collider.transform.position - context.Position;
            if (Physics.Raycast(context.Position, directiontoCol, out RaycastHit hit))
            {
                if (context.Settings.m_AvoidanceType == SteeringSettings.AvoidanceType.Ricochet)
                {
                    Vector3 Closetpoint = hit.point;
                    m_LastHitpos = Closetpoint;
                    Dir += (((Closetpoint - collider.transform.position)) * GetMultiPlyer(Vector3.Distance(context.Position, Closetpoint))) * Radius;
                }
                else
                {
                    Vector3 Hitpos = hit.point;
                    m_LastHitpos = Hitpos;
                    Dir += (((context.Position - Hitpos)) * GetMultiPlyer(Vector3.Distance(context.Position, Hitpos))) * Radius;
                }
                m_TargetPos = Dir + m_LastHitpos;
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

    private float GetPriorty(float distance, BehavorContext context)
    {
        float ammount = 0;
        if (distance <= Radius * 0.5f)
        {
            ammount = context.Settings.m_MaxPriorty;
            return ammount;
        }
        ammount = context.Settings.m_MinPriorty;
        return ammount;
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Color color = Color.blue;
        Support.DrawWiredSphere(behavorContext.Position, Radius, color);
        if (Priorty != 0)
        {
            color.a = 50f;
            Support.DrawLine(behavorContext.Position, behavorContext.Position + behavorContext.Velocity, Color.white);
            Support.DrawWiredSphere((m_PositionTarget - behavorContext.Position).normalized + behavorContext.Position, 1f, Color.red);
            DrawAvoid(behavorContext);
        }
    }

    private void DrawAvoid(BehavorContext Context)
    {
        Support.DrawLine(Context.Position, m_LastHitpos, Color.blue);
        Support.DrawLine(m_LastHitpos, m_TargetPos, Color.green);
    }
}