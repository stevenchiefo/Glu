using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private enum ControlSheme
    {
        keyboardandmouse,
        controler
    }

    [SerializeField] private GameObject m_Crossair;
    private GameManager m_GameManager;
    [SerializeField] public Vector3 m_Offset = new Vector3(0f, 0f, 0f);
    protected Controls inputActions;
    protected Vector2 RighStick;
    protected Camera m_MainCam;
    public string Name;
    protected Vector2 m_DirectionOfWalk;
    private Vector3 m_MousePostion;
    private Vector3 m_Heading;
    [SerializeField] private GameObject m_Bullit;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private float m_Speed = 0f;
    [SerializeField] private UIPlayerHealth m_UIPlayerHealth;
    [SerializeField] protected float m_Health = 6;
    [SerializeField] private Inventory[] m_Inventory = new Inventory[16];
    private PlayerMovement m_PlayerMovement;
    protected Rigidbody2D m_rigidbody2D;
    [SerializeField] public int Coins = 0;

    private void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_MainCam = Camera.main;
    }

    private void Start()
    {
        if (m_UIPlayerHealth == null)
        {
            Debug.LogError("there is no UiPlayerSelected");
        }
        m_Inventory[0] = new Inventory(new Potion("HealthPotion", Item.KindItem.Potion, 20));
    }

    // Update is called once per frame
    protected void Updater()
    {
        Mechanics();
        Death();
        Move();
        CameraFollow();
        CrossairFollow();
    }

    private void Mechanics()
    {
        UseItem();
    }

    private void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_Health += m_Inventory[0].Item.UseItem();
        }
    }

    private void Death()
    {
        if (m_Health <= 0)
        {
            Debug.Log("Player Died");
        }
    }

    public void GotHit()
    {
        m_Health -= 1;
        m_UIPlayerHealth.TookDamage();
    }

    private void LoadPlayer()
    {
        inputActions = new Controls();
        m_PlayerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    private void OnMove(InputValue value)
    {
        m_DirectionOfWalk = value.Get<Vector2>();
    }

    private void OnAim(InputValue value)
    {
        PlayerInput s = GetComponent<PlayerInput>();
        RighStick = value.Get<Vector2>();
        if (s.currentControlScheme == "Controler")
        {
            ControlerFollower();
        }
        else
        {
            MouseFollower();
        }
    }

    private void OnReset()
    {
        m_DirectionOfWalk = Vector2.zero;
    }

    protected void Move()
    {
        m_rigidbody2D.MovePosition(m_rigidbody2D.position + m_DirectionOfWalk * m_Speed * Time.deltaTime);
    }

    private void CameraFollow()
    {
        Vector3 EndPosition = new Vector3(transform.position.x, transform.position.y, m_MainCam.transform.position.z);
        m_MainCam.transform.position = Vector3.Lerp(m_MainCam.transform.position, EndPosition, 5f * Time.deltaTime);
    }

    private void OnShoot()
    {
        Shoot();
    }

    private void ControlerFollower()
    {
        Vector3 rightstick = RighStick;
        m_MousePostion = transform.position + (rightstick * 2f);
        Debug.Log(RighStick);
    }

    private void MouseFollower()
    {
        m_MousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePos = m_MousePostion;

        if (mousePos.x <= transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        if (mousePos.x >= transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void Shoot()
    {
        Fire();
    }

    private void Fire()
    {
        m_GameManager.ClickedPosition = m_MousePostion;
        Instantiate(m_Bullit, transform.position + m_Offset, transform.rotation);
    }

    private void CrossairFollow()
    {
        Vector3 mousepos = new Vector3(m_MousePostion.x, m_MousePostion.y, transform.position.z);
        m_Crossair.transform.position = mousepos;
    }
}