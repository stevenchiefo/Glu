using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private Controls m_InputActions;
    private Shader m_Material;

    private Vector2 m_Direction;

    private float m_ForcePower = 20f;
    [SerializeField] private float m_Speed = 0f;

    private void Awake()
    {
        m_Material = GetComponent<Shader>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_InputActions = new Controls();

        Color l = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
    }

    private void Update()
    {
        Move();
    }

    private void OnShoot()
    {
        Debug.Log("Shot");
    }

    private void OnMove(InputValue value)
    {
        Vector2 Direction = value.Get<Vector2>();
        m_Direction = Direction;
    }

    private void OnResetMove(InputValue value)
    {
        m_Direction = Vector2.zero;
    }

    private void Move()
    {
        Vector2 Direction = m_Direction;
        transform.Translate(Vector3.forward * Direction.y * m_Speed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.right * Direction.x * m_Speed * Time.deltaTime, Space.World);
        Debug.Log(Direction);
    }

    private void OnJump()
    {
        m_Rigidbody.AddForce(0f, m_ForcePower, 0f, ForceMode.Impulse);
    }

    private void OnEnable()
    {
        m_InputActions.Player.Enable();
    }

    private void OnDisable()
    {
        m_InputActions.Player.Disable();
    }
}