using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuster : ObjectToPool, IDamageble
{
    private enum Barrel
    {
        Barrel1 = 0,
        Barrel2 = 1,
    }

    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private Transform turret;
    [SerializeField] private Transform[] barrels;
    private DataManager DataManager;
    private Transform currentWaypoint;
    private new Rigidbody2D rigidbody;
    private float reloadTimer;
    private ObjectPool m_ObjectPool;
    private Barrel m_Barrel;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        DataManager = FindObjectOfType<DataManager>();
    }

    private void Start()
    {
        currentWaypoint = WaypointManager.instance.GetRandomWaypoint(); // Los de FindObjectOfType op met het Singleton Pattern
        m_Barrel = Barrel.Barrel1;
        gameObject.AddComponent<ObjectPool>();
        m_ObjectPool = GetComponent<ObjectPool>();
        m_ObjectPool.MakePool(5, bulletprefab);
        StartCoroutine(Shoot());
    }

    private void FixedUpdate()
    {
        RotateToWardsWayPoint();
    }

    private void Update()
    {
        CheckWayPoints();
        UpdateTurret();
    }

    private void RotateToWardsWayPoint()
    {
        if (currentWaypoint != null)
        {
            Vector3 relativePosition = currentWaypoint.position - transform.position;
            float angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, DataManager.instance.TankBusterData.vehicleTurningSpeed * Time.fixedDeltaTime);
            rigidbody.AddRelativeForce(Vector2.right * DataManager.instance.TankBusterData.drivingForce);
        }
    }

    private void CheckWayPoints()
    {
        reloadTimer = Mathf.Clamp(reloadTimer - Time.deltaTime, 0f, DataManager.instance.TankBusterData.reloadTime);
        if (Vector2.Distance(transform.position, currentWaypoint.position) <= DataManager.instance.TankBusterData.waypointRange)
        {
            currentWaypoint = WaypointManager.instance.GetNextWaypoint(currentWaypoint);
        }
    }

    private void UpdateTurret()
    {
        Player player = EntityManager.instance.Player;
        if (player != null)
        {
            Vector3 relativePosition = player.transform.position - turret.position;
            float angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, q, DataManager.instance.TankBusterData.turretRotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator Shoot()
    {
        do
        {
            yield return new WaitForSeconds(DataManager.instance.TankBusterData.reloadTime);
            reloadTimer += DataManager.instance.TankBusterData.reloadTime;
            GameObject bullet = m_ObjectPool.GetObject();
            Bullet script = bullet.GetComponent<Bullet>();
            int index = GetBarrel();
            script.SpawnObject(barrels[index].position, barrels[index].rotation);
        }
        while (true);
    }

    private int GetBarrel()
    {
        if (m_Barrel == Barrel.Barrel1)
        {
            m_Barrel = Barrel.Barrel2;
            return (int)m_Barrel;
        }
        else
        {
            m_Barrel = Barrel.Barrel1;
            return (int)m_Barrel;
        }
    }

    public void TakeDamage(int damage)
    {
        ScoreManager.instance.IncreaseScore(DataManager.instance.TankBusterData.score);
        PoolObject();
    }
}