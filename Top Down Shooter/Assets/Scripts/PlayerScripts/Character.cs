using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int Health = 0;
    public string Name;
    protected Vector2 m_DirectionOfWalk;

    [SerializeField] private float m_Speed = 0f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector2.up * m_DirectionOfWalk.y * m_Speed * Time.deltaTime, Space.World);
        transform.Translate(Vector2.right * m_DirectionOfWalk.x * m_Speed * Time.deltaTime, Space.World);
    }
}