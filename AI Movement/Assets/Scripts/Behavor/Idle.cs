using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steering;

public class Idle : Behavor
{
    public Idle()
    {
    }

    public override Vector3 CaculateSteeringForce(float dt, BehavorContext behavorContext)
    {
        return Vector3.zero;
    }
}