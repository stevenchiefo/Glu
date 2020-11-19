using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class ClickSeekPoint : Behavor
{
    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f))
            {
                m_PositionTarget = hit.point;
            }
        }
        m_VelocityDesired = (m_PositionTarget - behavorContext.Position) * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;


    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Support.DrawRay(behavorContext.Position, behavorContext.Velocity, Color.red);
        Support.Point(m_PositionTarget, 0.5f, Color.red);
    }
}
