using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageble
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] gunBarrels;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float reloadTime;
    [SerializeField] private float MissleReloadTime;
    [SerializeField] private int startingHealth;
    [SerializeField] private GameObject m_MisslePrefab;
    private SceneGameManager SceneGameManager;
    private new Rigidbody2D rigidbody;
    private new Camera camera;
    private Vector2 moveDirection;
    private bool firing;
    private bool missleCanFire;
    private float reloadTimer;
    private int currentSelectedBarrel;

    private ObjectPool ObjectPool;
    private ObjectPool m_MisslePool;

    private delegate void OnHit();

    private event OnHit OnPlayerHit;

    public int Health { get; private set; }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>().normalized;
    }

    public void ShootMissle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ActivateMissle();
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            firing = true;
        }
        else if (context.canceled)
        {
            firing = false;
        }
    }

    public void TakeDamage(int damage)
    {
        Health = Mathf.Clamp(Health - damage, 0, startingHealth);
        OnPlayerHit();
        if (Health == 0)
        {
            Die();
        }
    }

    private void Awake()
    {
        OnPlayerHit += FindObjectOfType<InterfaceManager>().UpdateInterFace;
        rigidbody = GetComponent<Rigidbody2D>();
        SceneGameManager = FindObjectOfType<SceneGameManager>();
        camera = Camera.main;
    }

    private void Start()
    {
        Health = startingHealth;

        gameObject.AddComponent<ObjectPool>();
        gameObject.AddComponent<ObjectPool>(); //Adding the components;

        ObjectPool[] objectPools = GetComponents<ObjectPool>(); // Getting the components en puttingen them in var's
        ObjectPool = objectPools[0];
        m_MisslePool = objectPools[1];

        ObjectPool.MakePool(5, bulletPrefab);
        m_MisslePool.MakePool(5, m_MisslePrefab); // Making the pool form ahead;

        OnPlayerHit();
        StartCoroutine(Timer());
    }

    private void Update()
    {
        CheckForShoot();
        RotateToWardsMouse();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        rigidbody.MovePosition(rigidbody.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    private void Die()
    {
        SceneGameManager.SetGameOver(); // Los de FindObjectOfType op met het Singleton Pattern
        gameObject.SetActive(false);
    }

    private void CheckForShoot()
    {
        reloadTimer = Mathf.Clamp(reloadTimer - Time.deltaTime, 0f, reloadTime);
        if (firing && reloadTimer == 0f)
        {
            GameObject bullet = ObjectPool.GetObject();
            Bullet script = bullet.GetComponent<Bullet>();
            script.SpawnObject(gunBarrels[currentSelectedBarrel].transform.position, transform.rotation);
            reloadTimer += reloadTime;
            currentSelectedBarrel++;
            if (currentSelectedBarrel >= gunBarrels.Length)
            {
                currentSelectedBarrel = 0;
            }
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(MissleReloadTime);
        missleCanFire = true;
    }

    private void ActivateMissle()
    {
        if (missleCanFire)
        {
            GameObject _obj = m_MisslePool.GetObject();
            Missle missle = _obj.GetComponent<Missle>();
            missle.SpawnObject(gunBarrels[currentSelectedBarrel].transform.position, transform.rotation);
            missleCanFire = false;
            StartCoroutine(Timer());
        }
    }

    private void RotateToWardsMouse()
    {
        Vector3 direction = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        float angle = (180f / Mathf.PI) * Mathf.Atan2(direction.y, direction.x);
        rigidbody.rotation = angle;
    }
}