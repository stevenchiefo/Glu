using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Persue : Behavor
{
    private Transform m_Target;
    private Vector3 m_Oldpos;
    private float m_LookAHeadTime = 2f;

    public Persue(Transform target)
    {
        m_Target = target;
        m_Oldpos = m_Target.position;
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        Vector3 TargetsDir = m_Target.position - m_Oldpos;
        Vector3 futurePos = m_Target.position + TargetsDir * m_LookAHeadTime;

        Vector3 dir = futurePos - behavorContext.Position;

        m_VelocityDesired = dir * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }
}