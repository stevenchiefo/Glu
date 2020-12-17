using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFarm : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    private float m_Xdir;
    private Rigidbody m_Rb;

    [Header("Shoot")]
    [SerializeField] private GameObject m_Projectile;
    [SerializeField] private Vector3 m_Offset;
    private ObjectPool m_ShootPool;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_ShootPool = gameObject.AddComponent<ObjectPool>();
        m_ShootPool.BeginPool(m_Projectile, 10, null);
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        Vector3 futurepos = transform.position + (Vector3.right * m_Xdir) * m_Speed * Time.deltaTime;
        if (InBoundsManager.Instance.InBounds(futurepos))
        {
            m_Rb.MovePosition(futurepos);
        }
    }

    private void CheckInput()
    {
        m_Xdir = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        PoolableObject poolableObject = m_ShootPool.GetObject();
        poolableObject.SpawnObject(transform.position + m_Offset);
    }
}