using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : ObjectToPool, IDamageble
{
    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private Transform turret;

    [SerializeField] private Transform barrel;

    private Transform currentWaypoint;
    private new Rigidbody2D rigidbody;
    private float reloadTimer;
    private ObjectPool m_ObjectPool;

    public void SetWaypoint(Transform waypoint)
    {
        currentWaypoint = waypoint;
    }

    public void TakeDamage(int damage)
    {
        ScoreManager.instance.IncreaseScore(DataManager.instance.EnemyData.score); // Los de FindObjectOfType op met het Singleton Pattern
        EntityManager.instance.currentEnemiesActive--; // Los de FindObjectOfType op met het Singleton Pattern
        PoolObject();
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentWaypoint = WaypointManager.instance.GetRandomWaypoint(); // Los de FindObjectOfType op met het Singleton Pattern
    }

    private void Start()
    {
        gameObject.AddComponent<ObjectPool>();
        m_ObjectPool = GetComponent<ObjectPool>();
        m_ObjectPool.MakePool(5, bulletprefab);
    }

    private void FixedUpdate()
    {
        if (currentWaypoint != null)
        {
            Vector3 relativePosition = currentWaypoint.position - transform.position;
            float angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, DataManager.instance.EnemyData.vehicleTurningSpeed * Time.fixedDeltaTime);
            rigidbody.AddRelativeForce(Vector2.right * DataManager.instance.EnemyData.drivingForce);
        }
    }

    private void Update()
    {
        reloadTimer = Mathf.Clamp(reloadTimer - Time.deltaTime, 0f, DataManager.instance.EnemyData.reloadTime);
        if (Vector2.Distance(transform.position, currentWaypoint.position) <= DataManager.instance.EnemyData.waypointRange)
        {
            currentWaypoint = WaypointManager.instance.GetNextWaypoint(currentWaypoint);
        }
        Player player = EntityManager.instance.Player;
        if (player != null)
        {
            Vector3 relativePosition = player.transform.position - turret.position;
            float angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, q, DataManager.instance.EnemyData.turretRotationSpeed * Time.deltaTime);
            if (reloadTimer == 0 && Vector2.Dot(relativePosition.normalized, turret.transform.right.normalized) == 1f)
            {
                reloadTimer += DataManager.instance.EnemyData.reloadTime;
                GameObject bullet = m_ObjectPool.GetObject();
                Bullet script = bullet.GetComponent<Bullet>();
                script.SpawnObject(barrel.position, barrel.rotation);
            }
        }
    }

    public override void ResetObject()
    {
    }
}