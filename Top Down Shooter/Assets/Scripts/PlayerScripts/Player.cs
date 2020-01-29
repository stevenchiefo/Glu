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

    private enum Animation
    {
        idle = 0,
        Running
    };

    private PlayerInput m_PlayerInput;
    [SerializeField] private GameObject m_Crossair;
    private GameManager m_GameManager;
    [SerializeField] public Vector3 m_Offset = new Vector3(0f, 0f, 0f);
    protected Controls inputActions;
    protected Vector2 RighStick;
    protected Camera m_MainCam;
    public string Name;
    protected Vector2 m_DirectionOfWalk;
    public Vector3 m_MousePostion;
    private float m_FramesPerSecondSave = 0f;
    [SerializeField] private float m_FramesPerSecond = 10f;
    [SerializeField] private GameObject m_Bullit;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private float m_Speed = 0f;
    [SerializeField] private UIPlayerHealth m_UIPlayerHealth;
    public float m_Health = 6;
    [SerializeField] private Inventory[] m_Inventory = new Inventory[16];

    public bool m_Ability1 = false;
    public bool m_Ability2 = false;
    public bool m_Ability3 = false;
    protected Rigidbody2D m_rigidbody2D;
    [SerializeField] public int Coins = 0;
    protected SpriteRenderer m_SpriteRenderer;
    private Animation m_AnimationStatus = Animation.idle;
    private Sprite[] m_Running;
    private Sprite[] m_Idle;
    private Rigidbody2D m_Rigidbody;

    private int m_AnimationCount = 0;

    private float m_AnimationTime = 0;

    [SerializeField] private string m_RunFilePath = "PlayerRun";
    [SerializeField] private string m_IdleFilePath = "PlayerIdle";

    private void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Running = Resources.LoadAll<Sprite>(m_RunFilePath);
        m_Idle = Resources.LoadAll<Sprite>(m_IdleFilePath);
        m_GameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_MainCam = Camera.main;
        m_FramesPerSecondSave = m_FramesPerSecond;
        GetCamera();
    }

    private void Start()
    {
        if (m_UIPlayerHealth == null)
        {
            Debug.LogError("there is no UiPlayerSelected");
        }
    }

    // Update is called once per frame
    protected void Updater()
    {
        Mechanics();
        Death();
        Move();
        AimSetter();
        CameraFollow();
        CrossairFollow();
        Render();
        m_UIPlayerHealth.SetScore(Coins);
    }

    private void Mechanics()
    {
        UseItem();
    }

    private void UseItem()
    {
    }

    private void Death()
    {
        if (m_Health <= 0)
        {
            Debug.Log("Player Died");
            m_Crossair.SetActive(false);
            spriteRenderer.enabled = false;
            enabled = false;
        }
    }

    public void GotHit()
    {
        m_Health -= 1;
        m_UIPlayerHealth.TookDamage();
    }

    private void GetCamera()
    {
        if (m_GameManager.GetPlayerCount() == 1)
        {
            m_GameManager.Players[0] = gameObject;

            return;
        }
        if (m_GameManager.GetPlayerCount() == 2)
        {
            m_GameManager.Players[1] = gameObject;
        }
    }

    private void Render()
    {
        if (m_DirectionOfWalk != Vector2.zero)
        {
            m_AnimationStatus = Animation.Running;
            m_FramesPerSecond = m_FramesPerSecondSave * 1.25f;
        }
        else if (m_AnimationStatus == Animation.Running)
        {
            m_AnimationStatus = Animation.idle;
            m_FramesPerSecond = m_FramesPerSecondSave;
        }

        Timer();
        if (m_AnimationStatus == Animation.idle)
        {
            if (m_AnimationCount >= m_Idle.Length - 1)
            {
                m_AnimationCount = 0;
            }

            m_SpriteRenderer.sprite = m_Idle[m_AnimationCount];
        }
        if (m_AnimationStatus == Animation.Running)
        {
            if (m_AnimationCount >= m_Running.Length - 1)
            {
                m_AnimationCount = 0;
            }
            m_SpriteRenderer.sprite = m_Running[m_AnimationCount];
        }
    }

    public void SetCam(Camera cam)
    {
        m_MainCam = cam;
    }

    public void SetUi(UIPlayerHealth h_)
    {
        m_UIPlayerHealth = h_;
    }

    private void Timer()
    {
        if (m_AnimationStatus == Animation.idle)
        {
            m_AnimationTime += Time.deltaTime;
            float End = 1f / m_FramesPerSecond;
            if (m_AnimationTime >= End)
            {
                m_AnimationTime = 0;
                m_AnimationCount++;
                if (m_AnimationCount >= m_Idle.Length - 1)
                {
                    m_AnimationCount = 0;
                    return;
                }
            }
        }
        if (m_AnimationStatus == Animation.Running)
        {
            m_AnimationTime += Time.deltaTime;
            float End = 1f / m_FramesPerSecond;
            if (m_AnimationTime >= End)
            {
                m_AnimationTime = 0;
                m_AnimationCount++;
                if (m_AnimationCount >= m_Running.Length - 1)
                {
                    m_AnimationCount = 0;
                    return;
                }
            }
        }
    }

    private void AimSetter()
    {
        PlayerInput s = GetComponent<PlayerInput>();
        if (s.currentControlScheme == "Controler")
        {
            ControlerFollower();
        }
        else
        {
            MouseFollower();
        }
    }

    protected virtual void Move()
    {
        m_rigidbody2D.MovePosition(m_rigidbody2D.position + m_DirectionOfWalk * m_Speed * Time.deltaTime);
    }

    private void CameraFollow()
    {
        Vector3 EndPosition = new Vector3(transform.position.x, transform.position.y, m_MainCam.transform.position.z);
        m_MainCam.transform.position = Vector3.Lerp(m_MainCam.transform.position, EndPosition, 5f * Time.deltaTime);
    }

    private void ControlerFollower()
    {
        Vector3 rightstick = RighStick;
        m_MousePostion = transform.position + (rightstick * 2f);
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

    private void MouseFollower()
    {
        m_MousePostion = m_MainCam.ScreenToWorldPoint(Input.mousePosition);
        m_MousePostion -= m_Offset;
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

    protected virtual void Ability1()
    {
    }

    protected virtual void Ability2()
    {
    }

    protected virtual void Ability3()
    {
    }

    private void Shoot()
    {
        Fire();
    }

    private void Fire()
    {
        m_GameManager.ClickedPosition = m_MousePostion + m_Offset;
        Instantiate(m_Bullit, transform.position + m_Offset, transform.rotation);
    }

    private void CrossairFollow()
    {
        if (RighStick == Vector2.zero)
        {
            m_Crossair.transform.position = transform.position + m_Offset;
        }
        else
        {
            Vector3 mousepos = new Vector3(m_MousePostion.x, m_MousePostion.y + m_Offset.y, transform.position.z);
            m_Crossair.transform.position = mousepos;
        }
    }

    #region Input

    public virtual void OnReset()
    {
        m_DirectionOfWalk = Vector2.zero;
    }

    private void OnShoot()
    {
        Shoot();
    }

    private void OnResetAim()
    {
        RighStick = Vector3.zero;
    }

    public virtual void OnMove(InputValue value)
    {
        m_DirectionOfWalk = value.Get<Vector2>();
    }

    public virtual void OnAim(InputValue value)
    {
        RighStick = value.Get<Vector2>();
    }

    private void OnAbility1()
    {
        Ability1();
    }

    #endregion Input
}