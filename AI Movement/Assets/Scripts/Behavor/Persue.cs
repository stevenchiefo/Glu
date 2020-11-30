using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Persue : Behavor
{
    private Transform m_Target;
    private Vector3 m_Oldpos;

    public Persue(Transform target)
    {
        m_Target = target;
        m_Oldpos = m_Target.position;
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        Vector3 TargetsDir = m_Target.position - m_Oldpos;
        Vector3 futurePos = m_Target.position + TargetsDir.normalized * behavorContext.Settings.m_MaxSpeed;
        m_PositionTarget = futurePos;

        Vector3 dir = m_PositionTarget - behavorContext.Position;

        m_Oldpos = m_Target.position;

        m_VelocityDesired = dir * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
        
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Support.DrawLine(behavorContext.Position, m_PositionTarget, Color.red);
        Support.DrawWiredSphere(m_PositionTarget, 1f, Color.red);
    }
}