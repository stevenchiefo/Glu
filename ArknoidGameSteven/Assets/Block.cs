using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private ParticleSystem m_ParticleSystem;
    private SpriteRenderer m_SpriteRenderer;
    private Collider2D m_Collider2D;

    private void Start()
    {
        m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_Collider2D = GetComponent<Collider2D>();
    }

    public void BallHit()
    {
        m_SpriteRenderer.enabled = false;
        m_Collider2D.enabled = false;
        m_ParticleSystem.startColor = m_SpriteRenderer.color;
        m_ParticleSystem.Play();
        StartCoroutine(StopParticleSystem());
    }

    public void ResetBlock()
    {
        m_SpriteRenderer.enabled = true;
        m_Collider2D.enabled = true;
    }

    private IEnumerator StopParticleSystem()
    {
        yield return new WaitForSeconds(0.5f);
        m_ParticleSystem.Stop();
    }
}