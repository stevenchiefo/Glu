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
    }
}
