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
    private Vector3 m_Destenation;

    public int Durrability { get; set; }

    //Movement

    private NavMeshAgent m_NavMeshAgent;
    private Rigidbody m_Rigidbody;

    //Combat
    private bool m_Combat;

    private bool m_MayFire;
    [SerializeField] private float m_AttackRange;

    //UI
    private EnemShipUI m_EnemyShipUI;

    public override void Load()
    {
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_EnemyShipUI = GetComponent<EnemShipUI>();

        m_NavMeshAgent.speed = DataBase.Instance.GetData().EnemyShipData.Speed;
        m_NavMeshAgent.angularSpeed = DataBase.Instance.GetData().EnemyShipData.RotationSpeed;
        Durrability = DataBase.Instance.GetData().EnemyShipData.MaxDurrabilty;

        m_EnemyShipUI.Load();
        OnPool.AddListener(OnObjectPooled);
    }

    private void Update()
    {
        CheckForCombat();
    }

    private void CheckForCombat()
    {
        if (Player.Instance.IsOnShip())
        {
            float _Distance = Vector3.Distance(PlayerShip.Instance.transform.position, transform.position);
            m_Combat = _Distance <= m_AttackRange;
        }
        else
        {
            m_Combat = false;
        }

        if (m_Combat)
        {
            m_NavMeshAgent.radius = 40;
        }
        else
        {
            m_NavMeshAgent.radius = 80;
        }
    }

    private IEnumerator CheckFire()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_MayFire == false);
            yield return new WaitForSeconds(DataBase.Instance.GetData().EnemyShipData.FireCooldown);
            m_MayFire = true;
        }
    }

    private IEnumerator CheckMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (m_Combat == false)
            {
                Move(m_Destenation);
            }
            else
            {
                Move(PlayerShip.Instance.GetClosestAttackPoint(transform.position));
                Shoot(AttackType.Forward);
            }
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
        if (m_NavMeshAgent.remainingDistance < 20f)
        {
            m_Destenation = EnitiyManager.Instance.GetNewPoint(vector3);
        }
        m_NavMeshAgent.SetDestination(vector3);
    }

    public void Rotate(Vector2 vector2)
    {
    }

    public void Shoot(AttackType attackType)
    {
        if (m_MayFire)
        {
            Transform _ClosestPoint = m_ShootPoints[GetIndexOfClosestShot()];
            Vector3 _Dir = _ClosestPoint.position - m_MiddlePoint.position;

            PoolableObject poolableObject = DataBase.Instance.GetCannonBall();
            poolableObject.SpawnObject(_ClosestPoint.position, _ClosestPoint.rotation);

            CannonBall cannonBall = poolableObject.GetComponent<CannonBall>();
            cannonBall.Launch(_Dir, DataBase.Instance.GetData().EnemyShipData.FirePower, CannonBall.TargetType.Player);
            m_MayFire = false;
        }
    }

    public void TakeDamage(int _Damage)
    {
        Durrability -= _Damage;
        if (Durrability <= 0)
        {
            TreasueChestData treasueChestData = new TreasueChestData
            {
                Gold = Random.Range(100, 500),
                CannonBalls = Random.Range(10, 30),
            };
            LootManager.Instance.SpawnTreasueChest(transform.position, transform.rotation, treasueChestData);
            PoolObject();
        }
        m_EnemyShipUI.UpdateUI();
    }

    private int GetIndexOfClosestShot()
    {
        Vector3 _PlayerPos = PlayerShip.Instance.transform.position;
        int index = 0;
        for (int i = 1; i < m_ShootPoints.Count; i++)
        {
            float Distance = Vector3.Distance(m_ShootPoints[i].position, _PlayerPos);
            float _IndexDistance = Vector3.Distance(m_ShootPoints[index].position, _PlayerPos);
            if (Distance < _IndexDistance)
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

        m_NavMeshAgent.speed = DataBase.Instance.GetData().EnemyShipData.Speed;
        m_NavMeshAgent.angularSpeed = DataBase.Instance.GetData().EnemyShipData.RotationSpeed;
        Durrability = DataBase.Instance.GetData().EnemyShipData.MaxDurrabilty;

        StartCoroutine(CheckMove());
        StartCoroutine(CheckFire());

        m_Destenation = EnitiyManager.Instance.GetNewPoint(Vector3.zero);
        m_EnemyShipUI.UpdateUI();
    }

    public Rigidbody GetRB()
    {
        return m_Rigidbody;
    }
}