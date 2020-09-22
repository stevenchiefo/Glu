using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : ObjectToPool
{
    private enum CharacterType
    {
        Player,
        Enemy,
        TankBuster,
    }

    private new Rigidbody2D rigidbody;
    private BulletData BulletData;
    [SerializeField] private CharacterType m_CharacterType;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetBulletData();
    }

    public void StartLifeTime()
    {
        Invoke("PoolObject", BulletData.lifeTime);
    }

    private void FixedUpdate()
    {
        if (BulletData != null)
        {
            Vector2 lastPosition = transform.position;
            rigidbody.MovePosition(rigidbody.position + (Vector2)transform.right * BulletData.speed * Time.fixedDeltaTime);
            RaycastHit2D raycastHit = Physics2D.Raycast(lastPosition, transform.right, (lastPosition - (Vector2)transform.position).magnitude, BulletData.hitLayerMask);
            if (raycastHit.collider != null)
            {
                IDamageble damageble = raycastHit.collider.GetComponent<IDamageble>();
                if (damageble != null)
                {
                    damageble.TakeDamage(BulletData.damage);
                    PoolObject();
                }
            }
        }
        else
        {
            SetBulletData();
        }
    }

    private void SetBulletData()
    {
        if (m_CharacterType == CharacterType.Player)
        {
            BulletData = DataManager.instance.PlayerBulletData;
        }
        if (m_CharacterType == CharacterType.Enemy)
        {
            BulletData = DataManager.instance.EnemyBulletData;
        }
        if (m_CharacterType == CharacterType.TankBuster)
        {
            BulletData = DataManager.instance.TankBustBulletData;
        }
    }
}