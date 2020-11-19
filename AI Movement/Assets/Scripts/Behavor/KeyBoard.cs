using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class KeyBoard : Behavor
{
    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (input != Vector3.zero)
        {
            m_PositionTarget = behavorContext.Position + input.normalized * behavorContext.Settings.m_MaxVelocityDesired;
        }

        m_VelocityDesired = (m_PositionTarget - behavorContext.Position) * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Support.DrawRay(behavorContext.Position, behavorContext.Velocity, Color.red);
        Support.Point(behavorContext.Velocity, 0.5f, Color.red);
    }
}
