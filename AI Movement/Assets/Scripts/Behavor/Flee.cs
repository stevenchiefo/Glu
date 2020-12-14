using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Flee : Behavor
{
    public Transform m_Target;
    private float m_Distance;
    public Flee(Transform Target)
    {
        m_Target = Target;
    }

    public Flee(Transform Target,float distance)
    {
        m_Target = Target;
        m_Distance = distance;
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        m_PositionTarget = m_Target.position;                                       //Declaring the m_PositionTarget;

        float _distance = Vector3.Distance(behavorContext.Position, m_Target.position);
        Vector3 dir = Vector3.zero;

        float neededDistance = m_Distance;
        if(neededDistance == 0)
        {
            m_Distance = behavorContext.Settings.m_FleeDistance;
        }

        if (_distance <= neededDistance)
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
    }
}
