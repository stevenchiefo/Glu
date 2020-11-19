using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Seek : Behavor
{
    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        m_PositionTarget = behavorContext.Position;

        m_VelocityDesired = (m_PositionTarget - behavorContext.Position) * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Support.DrawRay(behavorContext.Position, m_VelocityDesired, Color.red);
        Support.Point(m_VelocityDesired, 0.5f, Color.red);
    }
}
