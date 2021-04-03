using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using System;
using Unity.Collections;

public class PlayerMovementHandler : ComponentSystem
{
    private Entity m_Player;

    protected override void OnStartRunning()
    {
        //m_Player = GetEntityQuery(typeof(PlayerMoveData)).GetSingletonEntity();
        //PhysicsMass physicsMass = EntityManager.GetComponentData<PhysicsMass>(m_Player);
    }

    protected override void OnUpdate()
    {
        //PlayerMoveData _data = EntityManager.GetComponentData<PlayerMoveData>(m_Player);

        //bool _forward = Input.GetKey(_data.Forward);
        //bool _Back = Input.GetKey(_data.Backward);
        //bool _Right = Input.GetKey(_data.Right);
        //bool _Left = Input.GetKey(_data.Left);

        //int Z = Convert.ToInt32(_forward);
        //Z -= Convert.ToInt32(_Back);
        //int X = Convert.ToInt32(_Right);
        //X -= Convert.ToInt32(_Left);

        //Translation oldTrans = EntityManager.GetComponentData<Translation>(m_Player);
        //Rotation rotation = EntityManager.GetComponentData<Rotation>(m_Player);
        //float3 newpos = oldTrans.Value + (new float3(X, 0, Z) * _data.Speed * Time.DeltaTime);

        //rotation.Value.value.x = 0;
        //rotation.Value.value.z = 0;

        //EntityManager.SetComponentData(m_Player, rotation);

        //Translation translation = new Translation
        //{
        //    Value = newpos,
        //};
        //EntityManager.SetComponentData(m_Player, translation);
    }
}