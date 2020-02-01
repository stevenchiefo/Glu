using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public enum TargetStatus
    {
        Active = 0,
        InActive,
    }

    private ParticleSystem m_ParticleSystem;
    private TargetStatus m_TargetStatus;

    private void Start()
    {
        m_TargetStatus = TargetStatus.Active;
        m_ParticleSystem = GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_TargetStatus = TargetStatus.InActive;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        m_ParticleSystem.Play();
    }
}