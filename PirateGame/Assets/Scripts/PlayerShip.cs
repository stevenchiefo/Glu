using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour,  IShip
{
    public static PlayerShip Instance;

    //ShipStats
    public int Durrability { get; set; }
    


    [Header("ShootingPoints")]
    [SerializeField] private Transform m_FrontCannonTrans;
    [SerializeField] private Transform m_LeftCannonTrans;
    [SerializeField] private Transform m_RightCannonTrans;
    [SerializeField] private Transform m_MiddleFirePoint;

    //Ship other var's
    private Rigidbody m_RigidBody;
    private Player m_AssignedPlayer;
    private bool m_ShipCanMove;
    
    //Rotation

    private float m_RotationValue;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
    }


    private void Start()
    {
        m_RigidBody = GetComponentInChildren<Rigidbody>();
        Durrability = DataBase.Instance.GetData().ShipData.MaxDurrabilty;
    }



    public void SetCanMove(bool boolean)
    {
        m_ShipCanMove = boolean;
    }

    private void Update()
    {
        MoveShip();
        Rotate();
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

    public void Shoot(AttackType attackType)
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
    }

    private CannonBall.TargetType GetTargetType()
    {
        if (m_AssignedPlayer == null)
        {
            return CannonBall.TargetType.Player;
        }
        return CannonBall.TargetType.Enemy;
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
            m_AssignedPlayer.LeaveShip();
            SinkShip();
        }
    }

    private void SinkShip()
    {

    }

    public void Rotate(Vector2 vector2)
    {
        m_RotationValue = 0;
        m_RotationValue = vector2.x;
    }

    public void Move(Vector3 vector3)
    {
        
    }

    CannonBall.TargetType IShip.GetTargetType()
    {
        return CannonBall.TargetType.Enemy;
    }
}
