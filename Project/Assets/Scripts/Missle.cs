using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : ObjectToPool
{
    public LayerMask hitLayerMask;
    public LayerMask m_IngoreLayerMask;
    private Rigidbody2D m_Rigidbody;
    private Transform m_Target;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        AimToTarget();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public override void SpawnObject(Vector3 _pos, Quaternion _Rot)
    {
        m_Target = EntityManager.instance.GetClosedEnemy(transform.position);
        transform.position = _pos;
        transform.rotation = _Rot;
        gameObject.SetActive(true);
        Invoke("PoolObject", DataManager.instance.PlayerMissleData.lifeTime);
    }

    private void AimToTarget()
    {
        Vector3 relativePosition = m_Target.position - transform.position;
        float angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward); //Zo rotate hij met een bepaalde snelheid naar de target toe

        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, DataManager.instance.PlayerMissleData.RotationSpeed * Time.deltaTime); //Zo rotate hij met een bepaalde snelheid naar de target toe
    }

    private void Move()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + (Vector2)transform.right * DataManager.instance.PlayerMissleData.speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null || collision.gameObject.layer == m_IngoreLayerMask)
        {
            return;
        }

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, DataManager.instance.PlayerMissleData.Range, hitLayerMask);

        for (int i = 0; i < collider2Ds.Length; i++)
        {
            IDamageble damageble = collider2Ds[i].GetComponent<IDamageble>();
            if (damageble != null) // kijken of er iDamageble is en daarna gebruiken als het niet null is
            {
                damageble.TakeDamage(1);
            }
        }

        PoolObject();
    }
}