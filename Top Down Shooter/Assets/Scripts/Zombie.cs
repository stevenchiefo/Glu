using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public enum Animation
    {
        idle,
        run,
    }

    private Animation m_AnimationStatus;
    private string m_FilePathIdle = "Zombie/Idle";
    private string m_FilePathRun = "Zombie/Run";
    private Sprite[] m_RunAnimation;
    private Sprite[] m_IdleAnimation;
    private SpriteRenderer m_SpriteRenderer = null;
    [SerializeField] private float m_Distance = 5f;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField] private bool m_DidHit = false;
    [SerializeField] private float m_Speed = 10f;
    private Rigidbody2D m_Body;
    private RaycastHit2D m_Raycast;
    private Vector2 m_PlayerPos;
    private float m_AnimationTimer = 0;
    private int m_AnimationCounter = 0;
    [SerializeField] private float m_FramesPerSec = 10f;

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
        m_PlayerPos = FindObjectOfType<PlayerMovement>().gameObject.transform.localPosition;
        m_SpriteRenderer.flipX = isFlipped();
        AnimationPlayer();
        IsDeath();
    }

    private void FixedUpdate()
    {
        ShootRayCast();
    }

    private void ShootRayCast()
    {
        Vector2 playposv2 = m_PlayerPos;
        Vector2 tranfompostion = transform.position;
        Vector2 direction = tranfompostion - playposv2;
        direction = direction.normalized;

        Color Linecolor = Color.green;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -direction, m_Distance, m_LayerMask);
        m_Raycast = hit;
        if (m_Raycast.collider != null)
        {
            if (m_Raycast.collider.gameObject.tag == "Player")
            {
                Linecolor = Color.red;
                m_DidHit = true;
                MoveTopPlayer();
            }
            else
            {
                Linecolor = Color.green;
                m_DidHit = false;
            }
        }
        else
        {
            Linecolor = Color.green;
            m_DidHit = false;
        }
        Debug.DrawRay(tranfompostion, -direction, Linecolor);
    }

    private bool isFlipped()
    {
        return transform.position.x > m_PlayerPos.x;
    }

    private void MoveTopPlayer()
    {
        m_Body.MovePosition(Vector3.Lerp(transform.position, m_PlayerPos, m_Speed * Time.deltaTime));
    }

    private void LoadAnimations()
    {
        m_RunAnimation = Resources.LoadAll<Sprite>(m_FilePathRun);
        m_IdleAnimation = Resources.LoadAll<Sprite>(m_FilePathIdle);
    }

    private void AnimationPlayer()
    {
        m_AnimationTimer += Time.deltaTime;
        float endframe = 1f / m_FramesPerSec;
        if (m_AnimationTimer >= endframe)
        {
            m_AnimationTimer = 0;
            switch (m_AnimationStatus)
            {
                case Animation.idle:
                    if (m_AnimationCounter >= m_IdleAnimation.Length)
                    {
                        m_AnimationCounter = 0;
                    }
                    else
                    {
                        m_AnimationCounter++;
                    }

                    break;

                case Animation.run:
                    if (m_AnimationCounter >= m_RunAnimation.Length)
                    {
                        m_AnimationCounter = 0;
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
        }
    }
}