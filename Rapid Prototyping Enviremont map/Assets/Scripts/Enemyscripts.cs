using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemyscripts : MonoBehaviour
{
    private GameManager m_GameManager;
    private Vector3 m_Direction;
    private RaycastHit m_RaycastHit;
    private NavMeshAgent m_Agent;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField] private bool m_MoveToPlayer = false;
    private Rigidbody m_Body;
    [SerializeField] private float m_Speed = 10f;

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        m_Body = GetComponent<Rigidbody>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        SetDirection();
        Debug.DrawRay(transform.position, m_Direction);
    }

    private void FixedUpdate()
    {
        ShootRatCast();
        if (m_MoveToPlayer == true)
        {
            m_Agent.SetDestination(m_GameManager.GetPlayerObject().transform.position);
        }
    }

    private void SetDirection()
    {
        m_Direction = transform.position - m_GameManager.GetPlayerObject().transform.position;
        m_Direction = -m_Direction;
    }

    private void ShootRatCast()
    {
        if (Physics.Raycast(transform.position, m_Direction, out m_RaycastHit))
        {
            if (m_RaycastHit.collider.gameObject.tag == "Player")
            {
                m_MoveToPlayer = true;
            }
            else
            {
                m_MoveToPlayer = false;
            }
        }
    }

    private void Move()
    {
        m_Body.MovePosition(transform.position + m_Direction * m_Speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_GameManager.SpawnAtCheckPoint();
        }
    }
}