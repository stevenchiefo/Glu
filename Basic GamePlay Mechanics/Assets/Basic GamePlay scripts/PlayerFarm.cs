using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFarm : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    private float m_Xdir;
    private Rigidbody m_Rb;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        m_Rb.MovePosition(transform.position + (Vector3.left * m_Xdir) * m_Speed * Time.deltaTime);
    }

    private void CheckInput()
    {
        m_Xdir = Input.GetAxis("Horizontal");
    }
}