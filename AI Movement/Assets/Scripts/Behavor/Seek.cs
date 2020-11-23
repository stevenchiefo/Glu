using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Seek : Behavor
{
    public Transform m_Target;
    public Seek(Transform Target)
    {
        m_Target = Target;
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        float _distance = Vector3.Distance(behavorContext.Position, m_Target.position);
        Vector3 dir = Vector3.zero;
        if (_distance > behavorContext.Settings.m_StopDistance)
        {
            m_PositionTarget = m_Target.position;
            dir = m_PositionTarget - behavorContext.Position;
        }

        m_VelocityDesired = dir * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Vector3 posvelocity = behavorContext.Position + behavorContext.Velocity;
        Support.DrawLine(behavorContext.Position, posvelocity, Color.red);
        Support.Point(posvelocity, 0.5f, Color.red);
    }
}
