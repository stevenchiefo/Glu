using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class FollowPath : Behavor
{
    public List<Transform> m_WaitPoints;
    public int WaitPointIndex = 0;

    public bool Arrived;
    public UnityEvent OnArrived;

    public FollowPath(List<Transform> path)
    {
        m_WaitPoints = path;
    }

    public override void Start(BehavorContext behavorContext)
    {
        base.Start(behavorContext);
        OnArrived = new UnityEvent();
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        Vector3 dir = Vector3.zero;                                                //Making a local vector3 to Make it more readable;

        m_PositionTarget = GetNextPoint(behavorContext);                           //Declaring the m_PositionTarget;
        if (!Arrived)
        {
            dir = m_PositionTarget - behavorContext.Position;                       //Setting the direction;
            
            Vector3 toLineDir = StayWithPath(behavorContext);
            if (toLineDir != Vector3.zero)
            {
                dir += toLineDir;
            }

        }
        else
        {
            m_PositionTarget = behavorContext.Position;
        }


        m_VelocityDesired = dir * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }

    private Vector3 StayWithPath(BehavorContext context)
    {
        if (WaitPointIndex == 0)
            return Vector3.zero;


        Vector3 pos = GetPositionDir(context);
        float _Dis = Vector3.Distance(pos, context.Position);
        if (_Dis < context.Settings.m_PathDistance)
        {
            return Vector3.zero;
        }
        float Mulitplyer = GetMultiPlyer(_Dis);
        Vector3 dir = pos - context.Position;
        return dir * Mulitplyer;

    }

    private float GetMultiPlyer(float distance)
    {
        if (distance < 1)
        {
            return 2f + distance;
        }
        return 2f + (distance / 10f);
    }



    private Vector3 GetPositionDir(BehavorContext context)
    {
        Vector3 WaitPoint1 = m_WaitPoints[WaitPointIndex - 1].position;
        Vector3 WaitPoint2 = m_WaitPoints[WaitPointIndex].position;
        Vector3 pos = FindNearestPointOnLine(WaitPoint1, WaitPoint2, context.Position);
        Debug.DrawLine(context.Position, pos);
        return pos;
    }

    public Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 point)
    {
        //Get heading
        Vector3 heading = (end - origin);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        //Do projection from the point but clamp it
        Vector3 lhs = point - origin;
        float dotP = Vector3.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
    }

    private Vector3 GetNextPoint(BehavorContext Context)
    {
        float Distance = Vector3.Distance(Context.Position, m_WaitPoints[WaitPointIndex].position);

        if (Distance <= Context.Settings.m_PointArriveDistance)
        {
            if (WaitPointIndex < m_WaitPoints.Count - 1)
            {
                WaitPointIndex++;
            }
            else
            {
                Arrived = true;
                OnArrived.Invoke();
                return Context.Position;
            }
        }

        return m_WaitPoints[WaitPointIndex].position;
    }

    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        for (int i = 1; i < m_WaitPoints.Count; i++)
        {
            Support.DrawLine(m_WaitPoints[i - 1].position, m_WaitPoints[i].position, Color.blue);
        }
    }
}
