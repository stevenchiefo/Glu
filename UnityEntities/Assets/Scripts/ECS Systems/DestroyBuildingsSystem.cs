using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class DestroyBuildingsSystem : ComponentSystem
{
    public static DestroyBuildingsSystem Instance;

    protected override void OnCreate()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Enabled = false;
        }
    }

    protected override void OnUpdate()
    {
        
    }

    public void DestroyAllTags()
    {
        Entities.ForEach((ref DestoryTag _tag) =>
        {
            EntityManager.DestroyEntity(GetEntityQuery(typeof(DestoryTag)));
        });
    }
}
