using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCannon : MonoBehaviour
{
    [SerializeField] private GameObject m_Object;
    [SerializeField] private Transform m_Spawnpoint;
    [SerializeField] private float m_Power;

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(m_Spawnpoint.localPosition + Vector3.down));
    }

    public void Shoot()
    {
        if (m_Object != null)
        {
            Rigidbody _rig = m_Object.GetComponent<Rigidbody>();
            Vector3 dir = transform.TransformDirection(m_Spawnpoint.localPosition + Vector3.down);
            _rig.AddForce((dir * m_Power) * _rig.mass, ForceMode.Impulse);
            m_Object = null;
        }
    }
}