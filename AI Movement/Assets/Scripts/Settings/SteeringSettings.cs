using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [CreateAssetMenu(fileName = "Steering Settings", menuName = "Steering/Steering Settings", order = 1)]
    public class SteeringSettings : ScriptableObject
    {
        [Header("SteeringSettings")]
        public float m_Mass;

        public float m_MaxSteeringForce;
        public float m_MaxVelocityDesired;
        public float m_MaxSpeed;

        [Header("Braking")]
        public float m_StopDistance;

        [Header("Flee Setting")]
        public float m_FleeDistance;

        [Header("Pathing")]
        public float m_PathDistance;

        public float m_PointArriveDistance;

        [Header("Arrive")]
        public float m_ArriveDistance = 1f;

        public float m_SlowingDistance = 2f;

        [Header("Wander")]
        public float m_WanderCircleDistance = 5.0f;

        public float m_WanderCircleRadius = 5.0f;
        public float m_WanderNoiseAngle = 10f;

        [Header("ObjectAvoindance")]
        public float m_MaxPriorty = 1f;

        public float m_MinPriorty = 0.1f;
    }
}
