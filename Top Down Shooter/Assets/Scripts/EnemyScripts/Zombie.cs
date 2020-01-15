using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    private string m_FilePathExplosion = "Zombie/Explosion";

    private Sprite[] m_ExplodeAnimation;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Body = GetComponent<Rigidbody2D>();
        LoadAnimations();
        m_AnimationStatus = Animation.idle;
    }

    // Update is called once per frame
    private void Update()
    {
        Updater();
        if (Death == true && m_AnimationStatus != Animation.Explode)
        {
            m_AnimationStatus = Animation.Explode;
            m_AnimationCounter = 0;
        }
    }

    private void FixedUpdate()
    {
        if (Death == false)
        {
            ShootRayCast();
        }
    }

    private void LoadAnimations()
    {
        m_ExplodeAnimation = Resources.LoadAll<Sprite>(m_FilePathExplosion);
        m_RunAnimation = Resources.LoadAll<Sprite>(m_FilePathRun);
        m_IdleAnimation = Resources.LoadAll<Sprite>(m_FilePathIdle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.GotHit();
            m_AnimationStatus = Animation.Explode;
            m_AnimationCounter = 0;
            Death = true;
            Health = 0;
        }
    }

    protected override void AnimationPlayer()
    {
        m_AnimationTimer += Time.deltaTime;
        float endframe = 1f / m_FramesPerSec;
        if (m_AnimationTimer >= endframe)
        {
            m_AnimationTimer = 0;
            switch (m_AnimationStatus)
            {
                case Animation.idle:
                    if (m_AnimationCounter >= m_IdleAnimation.Length - 1)
                    {
                        m_AnimationCounter = 0;
                    }
                    else
                    {
                        m_AnimationCounter++;
                    }

                    break;

                case Animation.run:
                    if (m_AnimationCounter >= m_RunAnimation.Length - 1)
                    {
                        m_AnimationCounter = 0;
                    }
                    else
                    {
                        m_AnimationCounter++;
                    }
                    break;

                case Animation.Explode:
                    Collider2D collider = GetComponent<Collider2D>();
                    collider.enabled = false;
                    if (m_AnimationCounter >= m_ExplodeAnimation.Length - 1)
                    {
                        Die();
                    }
                    else
                    {
                        m_AnimationCounter++;
                    }
                    break;
            }
        }

        switch (m_AnimationStatus)
        {
            case Animation.idle:
                m_SpriteRenderer.sprite = m_IdleAnimation[m_AnimationCounter];
                break;

            case Animation.run:
                m_SpriteRenderer.sprite = m_RunAnimation[m_AnimationCounter];
                break;

            case Animation.Explode:
                m_SpriteRenderer.sprite = m_ExplodeAnimation[m_AnimationCounter];
                break;
        }
    }
}