using UnityEngine;

public class playermovement : MonoBehaviour
{
    public Rigidbody m_Player;
    public float m_speed;
    public float TurningSpeed;
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    private Camera m_Camera;
    private Vector2 m_ScreenCenter;
    private Vector2 m_MousePosition;
    private float m_LookX = 0.0f;
    private float m_LookY = 0.0f;
    private int m_CanJump = 2;
    [SerializeField] private float m_JumpSpeed;

    private void Start()
    {
        Cursor.visible = false;
        m_ScreenCenter = new Vector2(Screen.width, Screen.height);
        m_Camera = FindObjectOfType<Camera>();
        m_Camera.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        GetMousePosition();
        LookMovement();
        Movement();
    }

    private void Movement()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_CanJump >= 1)
        {
            m_CanJump--;
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.AddForce(0, (m_JumpSpeed * rigidbody.mass) * Time.deltaTime, 0, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * m_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * m_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * m_speed * Time.deltaTime);
        }
    }

    private void LookMovement()
    {
        if (Input.GetAxis("Mouse X") < 90)
            m_LookX += speedH * Input.GetAxis("Mouse X");

        m_LookY -= speedV * Input.GetAxis("Mouse Y");

        m_Camera.transform.eulerAngles = new Vector3(m_LookY, m_LookX, 0.0f);
        transform.transform.eulerAngles = new Vector3(0, m_LookX, 0.0f);
    }

    private void GetMousePosition()
    {
        m_MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnCollisionEnter(Collision info)
    {
        if (info.gameObject.tag == "floor")
        {
            m_CanJump = 2;
        }
    }
}