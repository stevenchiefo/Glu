using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Evade : Behavor
{
    private Transform m_Target;
    private Vector3 m_Oldpos;

    public Evade(Transform target)
    {
        m_Target = target;
        m_Oldpos = m_Target.position;
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {

        Vector3 TargetsDir = m_Target.position - m_Oldpos;
        Vector3 futurePos = m_Target.position + TargetsDir.normalized * behavorContext.Settings.m_MaxSpeed;
        Vector3 dir = behavorContext.Position - m_PositionTarget;
        m_PositionTarget = futurePos;
        m_Oldpos = m_Target.position;
        if (Vector3.Distance(futurePos, behavorContext.Position) > behavorContext.Settings.m_FleeDistance)
        {
            return Vector3.zero;

        }

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
