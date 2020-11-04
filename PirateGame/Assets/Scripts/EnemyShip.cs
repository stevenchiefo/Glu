using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyShip : PoolableObject, IShip
{
    

    [Header("ShootPoints")]
    [SerializeField] private List<Transform> m_ShootPoints;
    [SerializeField] private Transform m_MiddlePoint;

    public int Durrability { get; set; }

    //Movement

    private NavMeshAgent m_NavMeshAgent;

    public override void Load()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_NavMeshAgent.speed = DataBase.Instance.GetData().EnemyShipData.Speed;
        m_NavMeshAgent.angularSpeed = DataBase.Instance.GetData().EnemyShipData.RotationSpeed;
        Durrability = DataBase.Instance.GetData().EnemyShipData.MaxDurrabilty;

        OnPool.AddListener(OnObjectPooled);
    }
    

    private IEnumerator CheckMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
        }
    }

    public (Vector3 ForwardShoot, Vector3 SideRightWaysShoot, Vector3 SideLeftWaysShoot) GetShootingPoint()
    {
        throw new System.NotImplementedException();
    }

    public CannonBall.TargetType GetTargetType()
    {
        return CannonBall.TargetType.Player;
    }

    public void Move(Vector3 vector3)
    {
        m_NavMeshAgent.SetDestination(vector3);
    }

    public void Rotate(Vector2 vector2)
    {
        
    }

    public void Shoot(AttackType attackType)
    {
        Transform _ClosestPoint = m_ShootPoints[GetIndexOfClosestShot()];
        Vector3 _Dir = m_MiddlePoint.position - _ClosestPoint.position;
        PoolableObject poolableObject = DataBase.Instance.GetCannonBall();
        poolableObject.SpawnObject(_ClosestPoint.position, _ClosestPoint.rotation);
        CannonBall cannonBall = poolableObject.GetComponent<CannonBall>();
        cannonBall.Launch(_Dir, DataBase.Instance.GetData().EnemyShipData.FirePower, CannonBall.TargetType.Player);
    }

    public void TakeDamage(int _Damage)
    {
        Durrability -= _Damage;
        if(Durrability <= 0)
        {
            PoolObject();
        }
    }

    private int GetIndexOfClosestShot()
    {
        Vector3 _PlayerPos = PlayerShip.Instance.transform.position;
        int index = 0;
        for (int i = 1; i < m_ShootPoints.Count; i++)
        {
            float Distance = Vector3.Distance(m_ShootPoints[i].position, _PlayerPos);
            float _IndexDistance = Vector3.Distance(m_ShootPoints[index].position, _PlayerPos);
            if(Distance > _IndexDistance)
            {
                index = i;
            }
        }

        return index;
    }

    private void OnObjectPooled()
    {
        StopAllCoroutines();
    }

    public override void SpawnObject(Vector3 position, Quaternion rotation)
    {
        base.SpawnObject(position, rotation);
        StartCoroutine(CheckMove());
    }
}
