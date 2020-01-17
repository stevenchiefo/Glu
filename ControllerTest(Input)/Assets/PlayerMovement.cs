using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Controls m_InputActions;
    private string[] m_JoystickNames;
    private Joystick m_Joystick;

    [SerializeField] private float m_Speed = 0;
    private Vector2 m_Direction;

    private void Awake()
    {
        m_JoystickNames = Input.GetJoystickNames();

        m_InputActions = new Controls();

        m_InputActions.GamePlay.Move.performed += ctx => Move();
        m_InputActions.GamePlay.Move.canceled += ctx => m_Direction = Vector2.zero;
        m_InputActions.GamePlay.Shoot.performed += ctx => Shoot();
    }

    private void Update()
    {
    }

    private void Shoot()
    {
        Debug.Log("Shot" + " " + gameObject.name);
    }

    private void Move()
    {
        Vector2 direction = m_Direction;

        transform.Translate(Vector3.forward * direction.y * m_Speed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.right * direction.x * m_Speed * Time.deltaTime, Space.World);
        Debug.Log(direction + " " + gameObject.name);
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