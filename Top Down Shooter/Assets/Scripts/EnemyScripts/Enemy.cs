using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected enum Animation
    {
        idle,
        run,
        Explode,
    }

    public bool Death = false;

    [SerializeField] protected bool m_DidHit = false;

    [SerializeField] protected string m_FilePathIdle;
    [SerializeField] protected string m_FilePathRun;

    public string Name;

    [SerializeField] protected float m_Distance = 5f;
    [SerializeField] protected float m_Speed = 10f;

    public float m_FramesPerSec = 10f;
    public float Health;
    public float m_AnimationTimer = 0;
    public float Level;

    public int m_AnimationCounter = 0;

    protected Animation m_AnimationStatus;

    protected RaycastHit2D m_Raycast;

    protected Vector2 m_PlayerPos;
    protected Vector2 DirectionToPlayer;

    protected Rigidbody2D m_Body;

    public Sprite[] m_RunAnimation;
    public Sprite[] m_IdleAnimation;

    [SerializeField] protected LayerMask m_LayerMask;

    protected SpriteRenderer m_SpriteRenderer = null;

    private void Start()
    {
        if (Level != 0)
        {
            float multiplyer = Level / 10f + Level;
            Health = Health * multiplyer;
            Health = Mathf.Round(Health);
        }
    }

    protected virtual void Move()
    {
        m_Body.MovePosition(Vector3.Lerp(transform.position, m_PlayerPos, m_Speed * Time.deltaTime));
    }

    protected virtual void Attack()
    {
        Debug.Log(gameObject.name + " Does a attack");
    }

    protected void Updater()
    {
        try
        {
            m_PlayerPos = FindObjectOfType<Monster>().gameObject.transform.localPosition;
        }
        catch
        {
        }
        m_SpriteRenderer.flipX = isFlipped();
        AnimationPlayer();
        IsDeath();
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    private bool isFlipped()
    {
        return transform.position.x > m_PlayerPos.x;
    }

    public void TakeDamage(float damage)
    {
        this.Health -= damage;
    }

    protected void ShootRayCast()
    {
        Vector2 playposv2 = m_PlayerPos;
        Vector2 tranfompostion = transform.position;
        Vector2 direction = tranfompostion - playposv2;
        direction = direction.normalized;
        DirectionToPlayer = -direction;
        Color Linecolor = Color.green;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -direction, m_Distance, m_LayerMask);
        m_Raycast = hit;
        if (m_Raycast.collider != null)
        {
            if (m_Raycast.collider.gameObject.tag == "Player")
            {
                Linecolor = Color.red;
                m_DidHit = true;
                Move();
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

    protected virtual void Load()
    {
        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_Body = GetComponent<Rigidbody2D>();
        LoadAnimations();
        m_AnimationStatus = Animation.idle;
    }

    protected virtual void LoadAnimations()
    {
        m_RunAnimation = Resources.LoadAll<Sprite>(m_FilePathRun);
        m_IdleAnimation = Resources.LoadAll<Sprite>(m_FilePathIdle);
    }

    protected void IsDeath()
    {
        if (Health <= 0)
        {
            Death = true;
        }
    }

    protected virtual void AnimationPlayer()
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