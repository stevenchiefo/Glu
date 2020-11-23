using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Flee : Behavor
{
    public Transform m_Target;
    public Flee(Transform Target)
    {
        m_Target = Target;
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        m_PositionTarget = m_Target.position;                                       //Declaring the m_PositionTarget;

        float _distance = Vector3.Distance(behavorContext.Position, m_Target.position);
        Vector3 dir = Vector3.zero;

        if (_distance <= behavorContext.Settings.m_FleeDistance)
        {

            dir = m_PositionTarget - behavorContext.Position;                       //Making a local vector3 to Make it more readable;
        }
        else
        {
            return -behavorContext.Velocity;
        }


        m_VelocityDesired = -dir * behavorContext.Settings.m_MaxVelocityDesired;    //Setting the dir to nagivate to Go the other way;
        return m_VelocityDesired - behavorContext.Velocity;                         //Returen the velocity
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Support.DrawLine(behavorContext.Position, behavorContext.Position + behavorContext.Velocity, Color.blue);
        Support.DrawLine(behavorContext.Position, m_VelocityDesired, Color.red);
    }
}
