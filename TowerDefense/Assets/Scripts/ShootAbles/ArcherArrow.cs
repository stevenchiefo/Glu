using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.MPE;
using UnityEngine;

public class ArcherArrow : PoolableObject, IShootAble
{
    public Transform Target { get; set; }
    private Rigidbody m_Rigidbody;

    public override void Load()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    public int GetDamage()
    {
        return DataBase.Instance.GetShootAbleData(ShootAbleType.ArcherArrow).Damage;
    }

    public float GetSpeed()
    {
        return DataBase.Instance.GetShootAbleData(ShootAbleType.ArcherArrow).Speed;
    }

    public void SetTarget(Transform _Target)
    {
        Target = _Target;
    }

    private void FixedUpdate()
    {
        if (DataBase.Instance.GetShootAbleData(ShootAbleType.ArcherArrow).FollowTarget)
        {
            MoveObject();
        }
    }

    private void MoveObject()
    {
        if (Target != null)
        {
            Vector3 _dir = Target.position - transform.position;
            m_Rigidbody.AddForce(_dir * GetSpeed() * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            Debug.LogError($"Target wasn't assigned but follow is on!");
        }
    }

    public void FireObject(Vector3 direction)
    {
        m_Rigidbody.velocity = Vector3.zero;
        direction = direction.normalized;
        m_Rigidbody.AddForce(direction * GetSpeed(), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        HitEnemy(other);
        CheckForMap(other);
    }

    private void CheckForMap(Collider collider)
    {
        if(collider.gameObject.tag == "Map")
        {
            PoolObject();
        }
    }

    private void HitEnemy(Collider collider)
    {
        IEnemy enemy = collider.GetComponentInParent<IEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(GetDamage());
            PoolObject();
        }
    }
}
