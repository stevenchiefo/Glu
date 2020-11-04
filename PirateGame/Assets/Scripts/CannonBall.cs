using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEditorInternal;
using UnityEngine;

public class CannonBall : PoolableObject
{
    public enum TargetType
    {
        Player,
        Enemy,
    }

    private Rigidbody m_RigidBody;
    private TargetType m_Target;
    

    private void OnTriggerEnter(Collider other)
    {
        CheckForShip(other);
    }

    private void Update()
    {
        if (transform.position.y < 0)
        {
            PoolObject();
        }
    }

    private void CheckForShip(Collider collider)
    {
        IShip ship = collider.GetComponentInParent<IShip>();
        if (ship != null)
        {
            if (ship.GetTargetType() == m_Target)
            {
                ship.TakeDamage(DataBase.Instance.GetData().CannonBallData.Damage);
            }
            
        }
    }



    public void Launch(Vector3 _dir, float Power, TargetType targetType)
    {
        _dir = _dir.normalized;
        m_RigidBody.velocity = ((_dir * Power) * 20f) * Time.deltaTime;
        m_Target = targetType;
    }

    public override void Load()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }
}
