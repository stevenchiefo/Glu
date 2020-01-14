using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private float m_Distance;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField] private bool m_DidHit = false;
    private Rigidbody2D m_Body;
    private RaycastHit2D m_Raycast;
    private Vector2 m_PlayerPos;

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        m_PlayerPos = FindObjectOfType<PlayerMovement>().gameObject.transform.localPosition;
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
                MoveTopPlayer(-direction);
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

    private void MoveTopPlayer(Vector2 Direction)
    {
        m_Body.MovePosition(Direction);
    }
}