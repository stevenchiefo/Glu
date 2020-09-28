using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody m_Player;
    public float m_speed;
    public float TurningSpeed;
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    [SerializeField] private float m_LookSpeed;
    private Camera m_Camera;
    private Vector2 m_ScreenCenter;
    private Vector2 m_MousePosition;
    private float m_LookX = 0.0f;
    private float m_LookY = 0.0f;
    private int m_CanJump = 2;
    public bool IsRunning { private set; get; }

    private Vector3 m_MoveDirection;

    [SerializeField] private float m_MaxLookUp;
    [SerializeField] private float m_MaxLookDown;
    [SerializeField] private float m_JumpSpeed;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_ScreenCenter = new Vector2(Screen.width, Screen.height);
        m_Camera = Camera.main;
        m_Camera.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Look();
        Move();
    }

    private void Move()
    {
        transform.Translate(m_MoveDirection);
    }

    private void Look()
    {
        Cursor.visible = false;
        Vector2 mouseMovement = m_MousePosition;
        mouseMovement = mouseMovement * m_LookSpeed;

        float magnitude = mouseMovement.magnitude;

        mouseMovement = mouseMovement.normalized * magnitude;
        float mouseY = -mouseMovement.y * Time.deltaTime * 150f;
        Vector3 eulerAngles = new Vector3(m_Camera.transform.localEulerAngles.x + mouseY, mouseMovement.x, 0f);
        transform.eulerAngles = transform.eulerAngles + Vector3.up * mouseMovement.x * Time.deltaTime * 150f;

        if (eulerAngles.x < m_MaxLookUp && eulerAngles.x > 200f)
        {
            eulerAngles.x = m_MaxLookUp;
        }
        else if (eulerAngles.x > m_MaxLookDown && eulerAngles.x < 160f)
        {
            eulerAngles.x = m_MaxLookDown;
        }
        m_Camera.transform.localEulerAngles = Vector3.right * eulerAngles.x;
    }

    public void LookX(InputAction.CallbackContext context)
    {
        m_MousePosition.x = context.ReadValue<float>();
    }

    public void LookY(InputAction.CallbackContext context)
    {
        m_MousePosition.y = context.ReadValue<float>();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 _ReadedValue = context.ReadValue<Vector2>();
        Vector3 movement = new Vector3(_ReadedValue.x, 0f, _ReadedValue.y);

        movement = (movement * m_speed) * Time.deltaTime;
        m_MoveDirection = movement;

        IsRunning = _ReadedValue != Vector2.zero;
    }
}