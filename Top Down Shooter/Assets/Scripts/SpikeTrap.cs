using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private Sprite[] m_AanimationSprites;
    private SpriteRenderer m_SpriteRenderer;
    private Collider2D m_Collider2D;
    private float m_Time = 0f;
    private int m_AnimationCounter = 0;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        m_Time += Time.deltaTime;
        float limit = 1f / 1f;
        if (m_Time > +limit)
        {
            m_Time = 0;
            m_AnimationCounter++;
        }
        if (m_AnimationCounter > m_AanimationSprites.Length - 2)
        {
            m_Collider2D.enabled = true;
        }
        else
        {
            m_Collider2D.enabled = false;
        }

        if (m_AnimationCounter >= m_AanimationSprites.Length)
        {
            m_AnimationCounter = 0;
        }
        m_SpriteRenderer.sprite = m_AanimationSprites[m_AnimationCounter];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player P = collision.gameObject.GetComponent<Player>();
            P.GotHit();
        }
    }
}