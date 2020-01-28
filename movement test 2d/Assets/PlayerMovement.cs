using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_WalkSpeed = 5f;
    private PlayerInput m_PlayerInput;
    private Rigidbody2D m_Body2D;
    private Animator m_Animator;

    private void Start()
    {
        m_Body2D = GetComponent<Rigidbody2D>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        m_Animator.SetBool("Attack", m_PlayerInput.Attack);
    }

    private void Move()
    {
        Vector2 Currentpos = new Vector2(transform.position.x, transform.position.y);
        m_Body2D.MovePosition(Currentpos + m_PlayerInput.MovementDirection * m_WalkSpeed * Time.deltaTime);
    }

    private void LoadAllComponents()
    {
        m_Body2D = GetComponent<Rigidbody2D>();
    }
}