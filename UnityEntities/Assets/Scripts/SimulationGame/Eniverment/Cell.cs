using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector3 WorldPosition { get { return transform.position; } }
    public ResourceType Resource;
    public bool CanBuild = true;

    public bool IsFurtile = false;
    public bool AlreadyHasAHarvestable = false;

    private float m_Yoffset;
    private object m_Object;

    public void AssignResource(ResourceType _type) => Resource = _type;

    public void SpawnObject(PoolableObject _object)
    {
        GrowObject _growObject = _object.GetComponent<GrowObject>();
        if (_growObject != null)
        {
            _object.SpawnObject(WorldPosition + new Vector3(0, m_Yoffset, 0));
            _growObject.Grow();

            AlreadyHasAHarvestable = true;
            _growObject.OnHarvest.AddListener(ResetCanGrow);
            CanBuild = false;

            m_Object = _growObject;
        }
    }

    public void BuildObject(PoolableObject _object)
    {
        _object.SpawnObject(WorldPosition + new Vector3(0, m_Yoffset, 0));
        m_Object = _object;
        CanBuild = false;
    }

    public GrowObject GetGrowObject() => (GrowObject)m_Object;

    public House GetHouse() => (House)m_Object;

    private void ResetCanGrow()
    {
        AlreadyHasAHarvestable = false;
    }

    private void Awake()
    {
        m_Yoffset = gameObject.GetComponent<Collider>().bounds.size.y / 2;
    }
}