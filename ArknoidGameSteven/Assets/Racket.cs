using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour
{
    private Rigidbody2D m_RigidBody2D;
    [SerializeField] private float m_RacketSpeed;
    void Start()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        CheckMovement();
    }

    private void CheckMovement()
    {
        float x = 0;
        if (Input.GetKey(KeyCode.A))
        {
            x += -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x += 1;
        }
        Vector2 _dir = new Vector2(x, 0f);
        m_RigidBody2D.velocity = _dir.normalized * m_RacketSpeed;
    }
}
