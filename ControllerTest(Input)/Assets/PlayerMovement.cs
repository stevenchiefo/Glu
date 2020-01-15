using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Controls m_InputActions;

    [SerializeField] private float m_Speed = 0;
    private Vector2 m_Direction;

    private void Awake()
    {
        m_InputActions = new Controls();
        m_InputActions.GamePlay.Shoot.performed += ctx => Shoot();
        m_InputActions.GamePlay.Move.performed += ctx => m_Direction = ctx.ReadValue<Vector2>();
        m_InputActions.GamePlay.Move.canceled += ctx => m_Direction = Vector2.zero;
    }

    private void Update()
    {
        Move();
    }

    private void Shoot()
    {
        Debug.Log("Shot");
    }

    private void Move()
    {
        Vector2 direction = m_Direction;

        transform.Translate(direction * m_Speed * Time.deltaTime, Space.World);
        Debug.Log(direction);
    }

    private void OnEnable()
    {
        m_InputActions.Enable();
    }

    private void OnDisable()
    {
        m_InputActions.Disable();
    }
}