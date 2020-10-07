﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float m_BallSpeed;
    private Rigidbody2D m_RigidBody2D;
    void Start()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_RigidBody2D.velocity = new Vector2(GetRandomXValue(), 1f).normalized * m_BallSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Racket racket = collision.collider.GetComponent<Racket>();
        if(racket != null)
        {
            float x = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            Vector2 _dir = new Vector2(x + m_RigidBody2D.velocity.x/2f, 1f).normalized;
            m_RigidBody2D.velocity = _dir * m_BallSpeed;
        }
    }

    private float GetRandomXValue()
    {
        return Random.Range(-1f, 1f);
    }
}
