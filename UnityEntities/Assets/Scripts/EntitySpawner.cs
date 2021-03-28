using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

public class EntitySpawner : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField] private GameObject m_PlayerPrefabs;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
    }
}