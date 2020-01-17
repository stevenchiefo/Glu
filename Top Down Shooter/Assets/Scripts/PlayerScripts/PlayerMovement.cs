using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum Animation
    {
        idle = 0,
        Running
    };

    private SpriteRenderer m_SpriteRenderer;
    private Animation m_AnimationStatus = Animation.idle;
    private Sprite[] m_Running;
    private Sprite[] m_Idle;
    private Rigidbody2D m_Rigidbody;
    private Camera m_MainCam;
    private float m_RunTim = 0;
    private float m_IdleTim = 0;

    [SerializeField] private int m_AnimationCount = 0;
    private int m_IdleAnimationCount = 0;

    [SerializeField] private float m_Walkspeed = 0.1f;

    // Start is called before the first frame update
    private void Awake()
    {
    }

    private void Start()
    {
        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_Running = Resources.LoadAll<Sprite>("PlayerRun");
        m_Idle = Resources.LoadAll<Sprite>("PlayerIdle");
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_MainCam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        MouseFollower();
        Timer();
        Render();
        CameraFollow();
    }

    private void FixedUpdate()
    {
        //Movement();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + Vector2.up * m_Walkspeed * Time.fixedDeltaTime);
            m_AnimationStatus = Animation.Running;
            return;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + Vector2.left * m_Walkspeed * Time.fixedDeltaTime);
            m_AnimationStatus = Animation.Running;
            return;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + Vector2.right * m_Walkspeed * Time.fixedDeltaTime);
            m_AnimationStatus = Animation.Running;
            return;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + Vector2.down * m_Walkspeed * Time.fixedDeltaTime);
            m_AnimationStatus = Animation.Running;
            return;
        }
        else
        {
            if (m_AnimationStatus != Animation.idle)
            {
                m_AnimationCount = 0;
            }
            if (m_AnimationStatus != Animation.idle)
            {
                m_AnimationStatus = Animation.idle;
            }
        }
    }

    private void OnShoot()
    {
        Debug.Log("Shoot");
    }

    private void CameraFollow()
    {
        Vector3 EndPosition = new Vector3(transform.position.x, transform.position.y, m_MainCam.transform.position.z);
        m_MainCam.transform.position = Vector3.Lerp(m_MainCam.transform.position, EndPosition, 5f * Time.deltaTime);
    }

    private void Render()
    {
        if (m_AnimationStatus == Animation.idle)
        {
            m_SpriteRenderer.sprite = m_Idle[m_IdleAnimationCount];
        }
        if (m_AnimationStatus == Animation.Running)
        {
            m_SpriteRenderer.sprite = m_Running[m_AnimationCount];
        }
    }

    private void Timer()
    {
        if (m_AnimationStatus == Animation.idle)
        {
            m_IdleTim += Time.deltaTime;
            float End = 1f / 8f;
            if (m_IdleTim >= End)
            {
                m_IdleTim = 0;
                if (m_IdleAnimationCount >= m_Idle.Length - 1)
                {
                    m_IdleAnimationCount = 0;
                    return;
                }
                m_IdleAnimationCount++;
            }
        }
        if (m_AnimationStatus == Animation.Running)
        {
            m_RunTim += Time.deltaTime;
            float End = 1f / 10f;
            if (m_RunTim >= End)
            {
                m_RunTim = 0;
                if (m_AnimationCount >= m_Running.Length - 1)
                {
                    m_AnimationCount = 0;
                    return;
                }
                m_AnimationCount++;
            }
        }
    }

    private void MouseFollower()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x <= transform.position.x)
        {
            m_SpriteRenderer.flipX = true;
        }
        if (mousePos.x >= transform.position.x)
        {
            m_SpriteRenderer.flipX = false;
        }
    }
}