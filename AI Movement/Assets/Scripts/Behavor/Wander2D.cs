using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Wander2D : Behavor
{
    private Vector2 m_OldDirection;

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        Vector2 RandomDir = RandomDir = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));

        if (behavorContext.Position.x < EnitiyManager.instance.m_MinPos.x || behavorContext.Position.y < EnitiyManager.instance.m_MinPos.y)
        {
            RandomDir = Vector3.zero - behavorContext.Position;
            RandomDir = RandomDir.normalized * 10;
        }
        
        if (behavorContext.Position.x > EnitiyManager.instance.m_MaxPos.x || behavorContext.Position.y > EnitiyManager.instance.m_MaxPos.y)
        {
            RandomDir = Vector3.zero - behavorContext.Position;
            RandomDir = RandomDir.normalized * 10;
        }
        





        Vector2 pos = behavorContext.Position;
        m_PositionTarget = pos + (m_OldDirection + RandomDir * 2f);


        Vector3 dir = m_PositionTarget - behavorContext.Position;
        m_OldDirection = RandomDir;

        m_VelocityDesired = dir * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }




}
