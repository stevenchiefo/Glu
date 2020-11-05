using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerShip : MonoBehaviour, IShip
{
    public static PlayerShip Instance;

    //ShipStats
    public int Durrability { get; set; }

    [Header("ShootingPoints")]
    [SerializeField] private Transform m_FrontCannonTrans;

    [SerializeField] private Transform m_LeftCannonTrans;
    [SerializeField] private Transform m_RightCannonTrans;
    [SerializeField] private Transform m_MiddleFirePoint;

    [SerializeField] private LineRenderer m_FrontLineRendener;
    [SerializeField] private LineRenderer m_LeftLineRendener;
    [SerializeField] private LineRenderer m_RightLineRendener;

    private bool m_MayFire;

    //Ship other var's
    private Rigidbody m_RigidBody;

    private Player m_AssignedPlayer;
    private bool m_ShipCanMove;

    //Rotation

    private float m_RotationValue;

    //For ai Info
    [SerializeField] private List<Transform> m_AttackPoints;

    //Animation
    private Animator m_Animator;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        m_RigidBody = GetComponentInChildren<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();

        Durrability = DataBase.Instance.GetData().ShipData.MaxDurrabilty;

        m_FrontLineRendener.enabled = false;
        m_LeftLineRendener.enabled = false;
        m_RightLineRendener.enabled = false;

        StartCoroutine(CheckFire());
    }

    public void SetCanMove(bool boolean)
    {
        m_ShipCanMove = boolean;
    }

    private void Update()
    {
        MoveShip();
        Rotate();
        CheckCannonLine();
    }

    private void CheckCannonLine()
    {
        if (m_ShipCanMove)
        {


            switch (m_AssignedPlayer.GetAttackType())
            {
                case AttackType.Forward:
                    m_FrontLineRendener.enabled = true;
                    m_LeftLineRendener.enabled = false;
                    m_RightLineRendener.enabled = false;
                    break;
                case AttackType.LeftSide:
                    m_FrontLineRendener.enabled = false;
                    m_LeftLineRendener.enabled = true;
                    m_RightLineRendener.enabled = false;

                    break;
                case AttackType.RightSide:
                    m_FrontLineRendener.enabled = false;
                    m_LeftLineRendener.enabled = false;
                    m_RightLineRendener.enabled = true;
                    break;

            }
        }
        
    }

    private void Rotate()
    {
        Vector3 newrotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + m_RotationValue, transform.eulerAngles.z);

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, newrotation, GetShipData().RotationSpeed * Time.deltaTime);
    }

    private void MoveShip()
    {
        if (m_ShipCanMove)
        {
            transform.Translate(Vector3.forward * GetShipData().Speed * Time.deltaTime);
        }
    }

    public (Vector3 ForwardShoot, Vector3 SideRightWaysShoot, Vector3 SideLeftWaysShoot) GetShootingPoint()
    {
        return (m_FrontCannonTrans.position, m_RightCannonTrans.position, m_LeftCannonTrans.position);
    }

    private IEnumerator CheckFire()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_MayFire == false);
            yield return new WaitForSeconds(GetShipData().FireCooldown);
            m_MayFire = true;
        }
    }

    public void Shoot(AttackType attackType)
    {
        if (m_MayFire && m_AssignedPlayer.GetPlayerStats().CannonBalls > 0)
        {
            PoolableObject poolableObject = DataBase.Instance.GetCannonBall();
            CannonBall cannonBall = poolableObject.GetComponent<CannonBall>();
            Vector3 _Dir = Vector3.zero;
            switch (attackType)
            {
                case AttackType.Forward:
                    poolableObject.SpawnObject(m_FrontCannonTrans.position, m_FrontCannonTrans.rotation);
                    _Dir = m_FrontCannonTrans.position - m_MiddleFirePoint.position;

                    break;

                case AttackType.RightSide:
                    poolableObject.SpawnObject(m_RightCannonTrans.position, m_RightCannonTrans.rotation);
                    _Dir = m_RightCannonTrans.position - m_MiddleFirePoint.position;

                    break;

                case AttackType.LeftSide:
                    poolableObject.SpawnObject(m_LeftCannonTrans.position, m_LeftCannonTrans.rotation);
                    _Dir = m_LeftCannonTrans.position - m_MiddleFirePoint.position;

                    break;
            }
            cannonBall.Launch(_Dir, GetShipData().FirePower, GetTargetType());
            m_AssignedPlayer.RemoveCannonBalls(1);
            m_MayFire = false;
            PlayerInterfaceUI.Instance.UpdateUI();
        }
    }

    private ShipData GetShipData()
    {
        if (m_AssignedPlayer == null)
        {
            return DataBase.Instance.GetData().EnemyShipData;
        }
        return DataBase.Instance.GetData().ShipData;
    }

    public void AssignPlayer(Player player)
    {
        m_AssignedPlayer = player;
    }

    public Rigidbody GetRigidbody()
    {
        return m_RigidBody;
    }

    public void TakeDamage(int _Damage)
    {
        Durrability -= _Damage;
        if (Durrability <= 0)
        {
            Durrability = 0;
            m_AssignedPlayer.LeaveShip();
            SinkShip();
        }
        PlayerShipUI.Instance.UpdateUI();
    }

    private void SinkShip()
    {
        m_FrontLineRendener.enabled = false;
        m_LeftLineRendener.enabled = false;
        m_RightLineRendener.enabled = false;

        
    }

    public void DestroyShip()
    {

    }

    public void DockShip()
    {
        m_FrontLineRendener.enabled = false;
        m_LeftLineRendener.enabled = false;
        m_RightLineRendener.enabled = false;
    }

    public void Rotate(Vector2 vector2)
    {
        m_RotationValue = 0;
        m_RotationValue = vector2.x;
    }

    public void Move(Vector3 vector3)
    {
    }

    public CannonBall.TargetType GetTargetType()
    {
        return CannonBall.TargetType.Enemy;
    }

    public Rigidbody GetRB()
    {
        return m_RigidBody;
    }

    public Vector3 GetClosestAttackPoint(Vector3 vector3)
    {
        int _index = 0;
        for (int i = 1; i < m_AttackPoints.Count; i++)
        {
            float _Distance = Vector3.Distance(m_AttackPoints[i].position, vector3);
            float _ClosestDistance = Vector3.Distance(m_AttackPoints[_index].position, vector3);
            if (_Distance < _ClosestDistance)
            {
                _index = i;
            }
        }
        return m_AttackPoints[_index].position;
    }

    public Player GetPlayer()
    {
        return m_AssignedPlayer;
    }
}