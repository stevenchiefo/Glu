using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    public abstract class Behavor : IBehavor
    {
        [Header("Behavor Runtime")]
        public Vector3 m_PositionTarget;
        public Vector3 m_VelocityDesired;

        public float Priorty { get; set; }
        public string Label { get; set; }

        public virtual void Start(BehavorContext behavorContext)
        {
            m_PositionTarget = behavorContext.Position;
        }

        public abstract Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext);

        public virtual void OnDrawGizmos(BehavorContext behavorContext)
        {
            
        }

        public virtual void SetPriorty(float Priorty)
        {
            this.Priorty = Priorty;
        }

        
    }
}
