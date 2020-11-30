using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Arrive : Behavor
{
    private Transform m_Target;

    private Vector3 TargetOffset;
    private float Distance;
    private float ClippedSpeed;
    private float RampedSpeed;

    public bool DoBrake;

    public Arrive(Transform target)
    {
        m_Target = target;

    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        m_PositionTarget = m_Target.position;

        Vector3 stopVector = (behavorContext.Position - m_PositionTarget).normalized * behavorContext.Settings.m_ArriveDistance;
        Vector3 stopPostition = m_PositionTarget + stopVector;

        TargetOffset = stopPostition - behavorContext.Position;
        Distance = TargetOffset.magnitude;
        DoBrake = behavorContext.Settings.m_SlowingDistance >= Vector3.Distance(behavorContext.Position, m_PositionTarget);
        if (DoBrake)
        {
            Debug.Log("Breaking");
        }
        RampedSpeed = behavorContext.Settings.m_MaxVelocityDesired * (Distance / behavorContext.Settings.m_SlowingDistance);
        ClippedSpeed = Mathf.Min(RampedSpeed, behavorContext.Settings.m_MaxVelocityDesired);
        m_VelocityDesired = (ClippedSpeed / Distance) * TargetOffset;
        return (m_VelocityDesired - behavorContext.Velocity) * behavorContext.Settings.m_MaxVelocityDesired;
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Support.DrawCircle(m_PositionTarget, behavorContext.Settings.m_ArriveDistance, Color.red);
    }
}
