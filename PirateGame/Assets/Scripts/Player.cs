using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;



public class Player : MonoBehaviour
{
    private enum PlayerMode
    {
        InShip,
        OutShip,
    }

    private PlayerMode m_PlayerMode;

    //player

    [SerializeField] private float m_RotationSpeed;
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    private Vector3 m_Direction;
    private float m_Speed = 10f;

    //Ship
    [SerializeField] private PlayerShip m_Ship;

    [SerializeField] private Transform m_PlayerStand;

    //CameraMovement
    [Header("CameraOptions")]
    [SerializeField] private Transform m_CameraTransform;

    [SerializeField] private Vector3 m_LocalCameraOffset;
    [SerializeField] private Vector3 m_CameraOffset;
    [SerializeField] private float m_MaxDistance;
    [SerializeField] private float m_MinDistance;
    private Camera m_Camera;
    private Vector2 m_MouseMovement;
    private float m_Distance = 60f;
    private float m_CameraRotationSpeed = 100f;
    private float m_ScrollSensitivity = 0.1f;

    private void Start()
    {
        m_PlayerMode = PlayerMode.OutShip;
        Cursor.visible = false;
        m_Camera = m_CameraTransform.GetComponentInChildren<Camera>();
        m_Collider = GetComponent<Collider>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Ship.AssignPlayer(this);
    }

    private void Update()
    {
        CheckShipMovement();
        MovePlayer();

        switch (m_PlayerMode)
        {
            case PlayerMode.InShip:
                m_CameraTransform.position = m_Ship.transform.position + m_CameraOffset;
                break;

            case PlayerMode.OutShip:
                m_CameraTransform.position = transform.position + m_CameraOffset;
                RotatePlayer();
                break;
        }
    }

    private void CheckShipMovement()
    {
        m_Ship.SetCanMove(m_PlayerMode == PlayerMode.InShip);
        if (m_PlayerMode == PlayerMode.InShip)
        {
            transform.position = m_PlayerStand.position;
        }
    }

    private void MovePlayer()
    {
        transform.Translate(m_Direction * m_Speed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        
        switch (m_PlayerMode)
        {
            case PlayerMode.InShip:
                m_Ship.Rotate(callbackContext.ReadValue<Vector2>());
                break;

            case PlayerMode.OutShip:
                Vector2 value = callbackContext.ReadValue<Vector2>();
                m_Direction = new Vector3(value.x, 0f, value.y);
                break;
        }
    }

    public void OnMouse(InputAction.CallbackContext callbackContext)
    {
        m_MouseMovement =  new Vector2(callbackContext.ReadValue<float>(),0f);
        RotateCamera();
        if (callbackContext.canceled)
        {
            m_MouseMovement = Vector2.zero;
        }
    }

    public void OnAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            switch (m_PlayerMode)
            {
                case PlayerMode.InShip:
                    m_Ship.Shoot(GetAttackType());
                    break;

                case PlayerMode.OutShip:
                    break;
            }
        }
    }

    private AttackType GetAttackType()
    {
        float ForwardDistance = Vector3.Distance(m_Camera.transform.position, m_Ship.GetShootingPoint().ForwardShoot);
        float LeftDistance = Vector3.Distance(m_Camera.transform.position, m_Ship.GetShootingPoint().SideLeftWaysShoot);
        float RightDistance = Vector3.Distance(m_Camera.transform.position, m_Ship.GetShootingPoint().SideRightWaysShoot);
        float[] _Distances = new float[]
        {
            ForwardDistance,
            LeftDistance,
            RightDistance,
        };
        float _Far = 0;
        for (int i = 0; i < _Distances.Length; i++)
        {
            if(_Far == 0)
            {
                _Far = _Distances[i];
                continue;
            }

            if(_Far < _Distances[i])
            {
                _Far = _Distances[i];
            }
        }

        
        if(_Far == ForwardDistance)
        {
            return AttackType.Forward;
        }
        if(_Far == LeftDistance)
        {
            return AttackType.LeftSide;
        }
        if(_Far == RightDistance)
        {
            return AttackType.RightSide;
        }
        return AttackType.Forward;

    }

    private void RotatePlayer()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, m_CameraTransform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckForShip(collision);
    }

    private void CheckForShip(Collision collision)
    {
        if (m_PlayerMode == PlayerMode.OutShip)
        {
            PlayerShip ship = collision.gameObject.GetComponentInParent<PlayerShip>();
            if (ship != null)
            {
                BoardShip();
            }
        }
    }

    private void BoardShip()
    {
        m_PlayerMode = PlayerMode.InShip;
        m_Collider.enabled = false;
        m_Rigidbody.useGravity = false;
        m_Rigidbody.detectCollisions = false;
        transform.parent = m_PlayerStand.transform;
        transform.position = m_PlayerStand.position;
        Quaternion quaternion = new Quaternion(transform.rotation.x, -m_PlayerStand.rotation.y, transform.rotation.z, transform.rotation.w);
        transform.rotation = quaternion;
    }

    public void LeaveShip()
    {
        m_PlayerMode = PlayerMode.OutShip;
        m_Collider.enabled = true;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.detectCollisions = true;
        transform.parent = null;
    }

    #region Camera

    private void RotateCamera()
    {
        

        Vector2 mouseMovement = m_MouseMovement;
        Vector3 eulerAngles = new Vector3(m_CameraTransform.eulerAngles.x, m_CameraTransform.eulerAngles.y + mouseMovement.x * (m_CameraRotationSpeed * Time.deltaTime), m_CameraTransform.eulerAngles.z);
        m_CameraTransform.eulerAngles = eulerAngles;
        switch (m_PlayerMode)
        {
            case PlayerMode.InShip:
                m_CameraTransform.position = m_Ship.transform.position + m_CameraOffset;
                break;

            case PlayerMode.OutShip:
                m_CameraTransform.position = transform.position + m_CameraOffset;
                RotatePlayer();
                break;
        }
    }

    private void ScrollCamera(float ammount)
    {
        if (MayScroll(ammount))
        {
            m_Distance += ammount * m_ScrollSensitivity;
        }
        Vector3 _ScrollOffset = new Vector3(0f, m_Distance, -m_Distance);
        m_Camera.transform.localPosition = _ScrollOffset + m_LocalCameraOffset;
    }

    public void OnScrollCamera(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            ScrollCamera(-callbackContext.ReadValue<float>());
        }
    }

    private bool MayScroll(float ammount)
    {
        float _Distance = m_Distance;

        if (_Distance <= m_MinDistance)
        {
            if (ammount > 0)
            {
                return true;
            }
        }
        if (_Distance >= m_MaxDistance)
        {
            if (ammount < 0)
            {
                return true;
            }
        }

        if (_Distance > m_MinDistance && _Distance < m_MaxDistance)
        {
            return true;
        }

        return false;
    }

    #endregion Camera
}