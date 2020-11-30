using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Wander : Behavor
{
    private float m_WanderAngle;
    private Vector3 m_CenterCirlce;

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        m_WanderAngle += Random.Range(-5.0f * behavorContext.Settings.m_WanderNoiseAngle * Mathf.Deg2Rad,
            5.0f * behavorContext.Settings.m_WanderNoiseAngle * Mathf.Deg2Rad);

        m_CenterCirlce = behavorContext.Position + behavorContext.Velocity.normalized * behavorContext.Settings.m_WanderCircleDistance;

        Vector3 offset = new Vector3(behavorContext.Settings.m_WanderCircleRadius * Mathf.Cos(m_WanderAngle), 0.0f, behavorContext.Settings.m_WanderCircleRadius * Mathf.Sin(m_WanderAngle));

        m_PositionTarget = m_CenterCirlce + offset;

        Vector3 dir = m_PositionTarget - behavorContext.Position;

        m_VelocityDesired = dir * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Support.DrawCircle(m_CenterCirlce, behavorContext.Settings.m_WanderCircleRadius, Color.green);
        Support.DrawLine(m_CenterCirlce, m_PositionTarget, Color.blue);
        Support.DrawLine(behavorContext.Position, m_CenterCirlce,Color.green);    
        Support.DrawLabel(m_PositionTarget, "Target", Color.blue);
    }
}
