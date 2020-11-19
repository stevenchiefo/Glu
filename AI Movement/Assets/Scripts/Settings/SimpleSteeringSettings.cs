using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [CreateAssetMenu(fileName = "Simple SteeringSettings", menuName = "Steering/Simple Steering Settings", order = 2)]
    public class SimpleSteeringSettings : ScriptableObject
    {
        [Header("SteeringSettings")]
        public float m_Mass;
        public float m_MaxSteeringForce;
        public float m_MaxVelocityDesired;
        public float m_MaxSpeed;
    }
}

