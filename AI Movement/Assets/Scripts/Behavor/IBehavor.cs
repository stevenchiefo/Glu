using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    public interface IBehavor
    {
        float Priorty { get; set; }
        string Label { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="behavorContext"></param>
        void Start(BehavorContext behavorContext);

        /// <summary>
        /// s
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="behavorContext"></param>
        /// <returns></returns>
        Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext);

        void SetPriorty(float Priorty);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behavorContext"></param>
        void OnDrawGizmos(BehavorContext behavorContext);
    }
}
