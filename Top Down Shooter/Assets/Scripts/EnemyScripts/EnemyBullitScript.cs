using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullitScript : MonoBehaviour
{
    private Rigidbody2D m_Body;
    [SerializeField] private float m_FlySpeed = 0f;
    [SerializeField] private float m_LifeTime = 0f;
    private float m_LifeTimeCounter = 0f;
    private float m_Timer = 0f;
    [SerializeField] private List<GameObject> m_GameObjectLayerMask = new List<GameObject>();

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        LifeTimerChecker();
    }

    private void LifeTimerChecker()
    {
        m_Timer += Time.deltaTime;
        float EndTimer = 1f / 1f;
        if (m_Timer >= EndTimer)
        {
            m_Timer = 0;
            m_LifeTimeCounter++;
        }
        if (m_LifeTimeCounter >= m_LifeTime)
        {
            Explode();
        }
    }

    public void SetDirection(Vector2 Diretion)
    {
        Diretion = Diretion.normalized;
        m_Body.AddForce(Diretion * m_FlySpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void Explode()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (GameObject i in m_GameObjectLayerMask)
        {
            if (collision.gameObject.tag == i.tag)
            {
                return;
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.GotHit();
        }
        Explode();
    }
}