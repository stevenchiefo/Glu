using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullit : MonoBehaviour
{
    [SerializeField] private Sprite[] m_Explosion;
    [SerializeField] private List<GameObject> m_OnHit = new List<GameObject>();
    [SerializeField] private float m_Lifetime = 10f;
    private int m_AnimationCount = 0;
    private float m_LifTimeCounter = 0;
    private float m_AnimationCounter = 0f;
    public float m_Damage = 25f;
    private bool m_ExplosionActive = false;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private float m_ForcePower = 10f;
    private Animation m_Animation;
    private Rigidbody2D m_Rb;
    private SpriteRenderer m_SpriteRenderer;
    public Vector2 m_Heading;

    // Start is called before the first frame update
    private void Start()
    {
        //m_Animation = GetComponent<Animation>();
        //m_Animation.Play();
        m_Rb = GetComponent<Rigidbody2D>();
        m_Heading = transform.position - FindObjectOfType<GameManager>().ClickedPosition;
        m_Heading = m_Heading.normalized;
        m_Rb.AddForce(-m_Heading * m_ForcePower, ForceMode2D.Impulse);
        m_Explosion = Resources.LoadAll<Sprite>("Ánimations");
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        m_LifTimeCounter += Time.deltaTime;
        if (m_LifTimeCounter >= m_Lifetime)
        {
            m_LifTimeCounter = 0;
            Explode();
        }
        if (m_ExplosionActive == true)
        {
            m_AnimationCounter += Time.deltaTime;
            float endtime = 1f / 10f;
            if (m_AnimationCounter >= endtime)
            {
                m_AnimationCount++;
                m_AnimationCounter = 0;
                if (m_AnimationCount >= m_Explosion.Length - 1)
                {
                    DestroyGameObject();
                }
            }
            m_SpriteRenderer.sprite = m_Explosion[m_AnimationCount];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (GameObject i in m_OnHit)
        {
            if (i.name == collision.gameObject.name)
            {
                return;
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            return;
        }

        if (collision.gameObject.tag == "Enemys")
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            e.TakeDamage(m_Damage);
        }
        Explode();
    }

    private void Explode()
    {
        m_ExplosionActive = true;
        m_Rb.velocity = Vector2.zero;
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}