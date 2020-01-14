using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullit : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_OnHit = new List<GameObject>();
    [SerializeField] private float m_Lifetime = 10f;
    private float m_LifTimeCounter = 0;
    [SerializeField] private float m_Damage = 25f;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private float m_ForcePower = 10f;
    private Animation m_Animation;
    private Rigidbody2D m_Rb;
    private Vector2 m_Heading;

    // Start is called before the first frame update
    private void Start()
    {
        //m_Animation = GetComponent<Animation>();
        //m_Animation.Play();
        m_Rb = GetComponent<Rigidbody2D>();
        m_Heading = transform.position - FindObjectOfType<GameManager>().ClickedPosition;
        m_Heading = m_Heading.normalized;
        m_Rb.AddForce(-m_Heading * m_ForcePower, ForceMode2D.Impulse);
    }

    private void Update()
    {
        m_LifTimeCounter += Time.deltaTime;
        if (m_LifTimeCounter >= m_Lifetime)
        {
            m_LifTimeCounter = 0;
            Destroy(gameObject);
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
        Destroy(gameObject);
    }
}