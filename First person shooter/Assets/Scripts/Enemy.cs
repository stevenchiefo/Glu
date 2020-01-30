using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_Health = 100;
    private GameObject m_Player;
    private RaycastHit m_RaycastHit;
    [SerializeField] private int m_Distance;
    [SerializeField] private LayerMask m_LayerMask;
    private bool m_PlayedDetected = false;

    private void Start()
    {
        m_Player = FindObjectOfType<PlayerMovment>().gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        DetectPlayer();
        LookAtPlayer();
        Death();
    }

    public void GotHit(float damage)
    {
        m_Health = m_Health - damage;
        Rigidbody r = gameObject.GetComponent<Rigidbody>();
        //r.AddRelativeForce(Vector3.back * (damage * 10));
    }

    private void Death()
    {
        if (m_Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool IsDeath()
    {
        return m_Health <= 0;
    }

    private void DetectPlayer()
    {
        Vector3 dir = m_Player.transform.position - transform.position;
        Physics.Raycast(transform.position, dir, out m_RaycastHit);

        if (m_RaycastHit.collider != null)
        {
            if (m_RaycastHit.collider.gameObject.CompareTag("Player"))
            {
                m_PlayedDetected = true;
            }
            else
            {
                m_PlayedDetected = false;
            }
        }
        Debug.DrawRay(transform.position, dir);
    }

    private void LookAtPlayer()
    {
        transform.LookAt(m_Player.transform);
    }
}