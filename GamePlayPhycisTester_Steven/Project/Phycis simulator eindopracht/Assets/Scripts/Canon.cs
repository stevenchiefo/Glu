using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    [SerializeField] private GameObject m_CanonBall;
    [SerializeField] private Transform m_Shootingpoint;
    [SerializeField] private float m_Force;
    [SerializeField] private float m_Speed;

    // Update is called once per framea
    private void Start()
    {
        Shoot();
    }

    private void ShootChecker()
    {
        if (m_CanonBall != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Vector3 dir = m_Shootingpoint.position - transform.position;
        dir = dir.normalized;
        Vector3 addyforce = new Vector3(0f, m_Force, 0f);
        Rigidbody rigidbody = m_CanonBall.GetComponent<Rigidbody>();
        float mass = rigidbody.mass;
        rigidbody.AddForce((dir * m_Speed + addyforce) * mass, ForceMode.Impulse);
        m_CanonBall = null;
    }
}