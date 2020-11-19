using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    public struct BehavorContext
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public SteeringSettings Settings;

        public BehavorContext(Vector3 position,Vector3 velocity,SteeringSettings settings)
        {
            Position = position;
            Velocity = velocity;
            Settings = settings;
        }
    }
}
