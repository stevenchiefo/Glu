using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [SerializeField] private float m_Speed;
    [SerializeField] private float m_MaxSpeed;

    [SerializeField] private CameraRotation m_CameraRotation;
    private Rigidbody m_Rb;

    private PowerUps m_PowerUpState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
        LimitSpeed();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveForward();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MoveBackWard();
        }
    }

    private void LimitSpeed()
    {
        m_Rb.velocity = Vector3.ClampMagnitude(m_Rb.velocity, m_MaxSpeed);
    }

    private void MoveForward()
    {
        Vector3 dir = m_CameraRotation.GetDirectionCameraIsFacing();
        m_Rb.AddForce(dir * m_Speed * Time.deltaTime);
    }

    private void MoveBackWard()
    {
        Vector3 dir = m_CameraRotation.GetDirectionCameraIsFacing();
        m_Rb.AddForce(-dir * m_Speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CheckForEnemy(collision))
        {
            PowerUpCheck(collision);
        }
    }

    private void PowerUpCheck(Collision collision)
    {
        if (m_PowerUpState == PowerUps.Bounce)
        {
            Rigidbody enemyrb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 dir = enemyrb.position - transform.position;
            enemyrb.AddForce(dir.normalized * 20, ForceMode.Impulse);
        }
    }

    public void SetPowerUp(PowerUps powerUps, float duration)
    {
        m_PowerUpState = powerUps;
        RemovePowerUp(duration);
    }

    private IEnumerator RemovePowerUp(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_PowerUpState = PowerUps.None;
    }

    private bool CheckForEnemy(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        return enemy != null;
    }
}

public enum PowerUps
{
    None,
    Bounce,
}