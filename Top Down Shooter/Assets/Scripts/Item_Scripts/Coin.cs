using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Sprite[] m_Sprites;
    private Player m_Player;
    private int m_AnimationCount = 0;
    private float m_Timer;

    public void Awake()
    {
        Load();
    }

    public void Update()
    {
        Render();
    }

    private void Load()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        m_Player = p.GetComponent<Player>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Sprites = Resources.LoadAll<Sprite>("Coin");
    }

    private void Render()
    {
        m_SpriteRenderer.sprite = m_Sprites[m_AnimationCount];

        m_Timer += Time.deltaTime;
        float End = 1f / 8f;
        if (m_Timer >= End)
        {
            m_Timer = 0;
            if (m_AnimationCount >= m_Sprites.Length - 1)
            {
                m_AnimationCount = 0;
                return;
            }
            m_AnimationCount++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_Player.Coins += 1;
            Destroy(gameObject);
        }
    }
}