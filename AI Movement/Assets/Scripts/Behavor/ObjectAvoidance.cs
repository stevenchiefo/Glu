using Steering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAvoidance : Behavor
{
    public float Radius;
    private LayerMask LayerMask;
    public ObjectAvoidance(float radius, LayerMask layerMask)
    {
        Radius = radius;
        LayerMask = layerMask;
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        Debug.Log(Priorty);
        if (Priorty == 0)
        {
            return Vector3.zero;
        }
        m_PositionTarget = GetAllDir(behavorContext);
        Vector3 dir = m_PositionTarget - behavorContext.Position;


        m_VelocityDesired = dir * behavorContext.Settings.m_MaxVelocityDesired;
        return m_VelocityDesired - behavorContext.Velocity;
    }

    public float CaculatePriorty(BehavorContext context)
    {
        float priorty = 0.0f;
        Collider[] Colliders = Physics.OverlapSphere(context.Position, Radius, LayerMask);
        if (Colliders.Length > 0)
        {
            Debug.Log("hit");
            foreach (Collider col in Colliders)
            {

                Vector3 dir = col.transform.position - context.Position;
                if (Physics.Raycast(context.Position, dir, out RaycastHit hit))
                {

                    priorty = GetPriorty(Vector3.Distance(context.Position, hit.point));
                    Priorty = priorty;
                    return priorty;
                }


            }

        }
        Priorty = priorty;
        return priorty;
    }

    private Vector3 GetAllDir(BehavorContext context)
    {
        Collider[] Colliders = Physics.OverlapSphere(context.Position, Radius, LayerMask);
        Vector3 Dir = Vector3.zero;
        foreach (Collider collider in Colliders)
        {
            if (collider.tag.ToLower() != "ground")
            {

                Vector3 directiontoCol = collider.transform.position - context.Position;
                if (Physics.Raycast(context.Position, directiontoCol, out RaycastHit hit))
                {
                    Debug.DrawLine(context.Position, hit.point, Color.cyan);
                    Dir += ((context.Position - collider.transform.position + hit.point) * Priorty ) * GetMultiPlyer(Vector3.Distance(context.Position, hit.collider.ClosestPoint(hit.point)));
                }
            }
        }
        Debug.DrawLine(context.Position, context.Position + Dir);
        return Dir;
    }
    private float GetMultiPlyer(float distance)
    {
        float ammount = 0;
        if (distance < 1)
        {
            ammount = 1 - distance * 1.5f;
            return 2f + ammount;
        }
        ammount = 1 - (distance * 1.5f / 10f);
        return 2f + ammount;
    }

    private float GetPriorty(float distance)
    {
        
        float ammount = 0;
        if (distance < Radius * 0.95f)
        {
            ammount = 0.9f;
            return ammount;
        }
        ammount = 0.6f;
        return ammount;
    }


    public override void OnDrawGizmos(BehavorContext behavorContext)
    {
        base.OnDrawGizmos(behavorContext);
        Color color = Color.blue;
        color.a = 50f;
        Support.DrawLine(behavorContext.Position, behavorContext.Position + behavorContext.Velocity, Color.white);
        Support.DrawWiredSphere(behavorContext.Position, Radius, color);
    }


}
