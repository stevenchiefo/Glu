using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

public enum UpgradeStatus
{
    ShootOne,
    TripleShoot,
    Spread,
    Burst,
}

public class ShipController : MonoBehaviour
{
    public static ShipController Instance;

    [Header("Movement:")]
    public float moveSpeed = 10.0f; // The players movement speed

    [Header("Weapons:")]
    public GameObject playerBullet;                        // Reference to the players bullet prefab

    public GameObject startWeapon;                         // The players initial 'turret' gameobject
    public List<GameObject> tripleShotTurrets;                   //
    public List<GameObject> wideShotTurrets;                     // References to the upgrade weapon turrets
    public List<GameObject> scatterShotTurrets;                  //
    public List<GameObject> activePlayerTurrets;                 //
    public float scatterShotTurretReloadTime = 2.0f;  // Reload time for the scatter shot turret!

    [Header("Effects:")]
    public GameObject explosion;                           // Reference to the Explosion prefab

    public ParticleSystem playerThrust;                        // The particle effect for the ships thruster

    [Header("Debug:")]
    public bool godMode = false; // Set to true to enable god mode (no game over)

    public int upgradeState = 0;     // A reference to the upgrade state of the player

    // private stuff
    private Rigidbody2D playerRigidbody;                     // The players rigidbody: Required to apply directional force to move the player

    private Renderer playerRenderer;                      // The Renderer for the players ship sprite
    private CircleCollider2D playerCollider;                      // The Players ship collider
    private AudioSource shootSoundFX;                        // The player shooting sound effect

    // Added by steven
    //
    // Main class value's
    private Vector2 m_Direction = Vector2.zero;

    private int m_Health;
    public int m_MaxHealth;

    private UpgradeStatus m_UpgradeStatus;

    // Main pool's
    private ObjectPool m_BulletPool;

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
        StartFromTeachers();   //voor meer overzicht had ik de start dat er al in zat in een function gedaan
        SetPools();            //Maak de objectPools aan
        m_Health = m_MaxHealth;
    }

    public int GetHealth()
    {
        return m_Health;
    }

    private void StartFromTeachers()
    {
        playerCollider = gameObject.GetComponent<CircleCollider2D>();
        playerRenderer = gameObject.GetComponent<Renderer>();
        activePlayerTurrets = new List<GameObject> { startWeapon };
        shootSoundFX = gameObject.GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void SetPools()
    {
        m_BulletPool = gameObject.AddComponent<ObjectPool>();
        m_BulletPool.BeginPool(playerBullet, 10, null);
    }

    private void MovePlayer()
    {
        playerRigidbody.velocity = m_Direction * moveSpeed;
        playerRigidbody.position = BoundaryManager.Instance.Clamp(playerRigidbody.position);
    }

    private void ShootOne()
    {
        PoolableObject poolable = m_BulletPool.GetObject();
        poolable.SpawnObject(startWeapon.transform.position, startWeapon.transform.rotation);
        poolable.LifeTimeTimer(1);
    }

    private void ShootTriple()
    {
        for (int i = 0; i < tripleShotTurrets.Count; i++)
        {
            PoolableObject poolable = m_BulletPool.GetObject();
            poolable.SpawnObject(tripleShotTurrets[i].transform.position, tripleShotTurrets[i].transform.rotation);
            poolable.LifeTimeTimer(1);
        }
    }

    private void ShootSpread()
    {
        for (int i = 0; i < wideShotTurrets.Count; i++)
        {
            PoolableObject poolable = m_BulletPool.GetObject();
            poolable.SpawnObject(wideShotTurrets[i].transform.position, wideShotTurrets[i].transform.rotation);
            poolable.LifeTimeTimer(1);
        }
    }

    private void ShootBurst()
    {
        for (int i = 0; i < scatterShotTurrets.Count; i++)
        {
            PoolableObject poolable = m_BulletPool.GetObject();
            poolable.SpawnObject(scatterShotTurrets[i].transform.position, scatterShotTurrets[i].transform.rotation);
            poolable.LifeTimeTimer(1);
        }
    }

    public void AddUpgrade()
    {
        switch (m_UpgradeStatus)
        {
            case UpgradeStatus.ShootOne:

                m_UpgradeStatus = UpgradeStatus.TripleShoot;
                break;

            case UpgradeStatus.TripleShoot:
                m_UpgradeStatus = UpgradeStatus.Spread;
                break;

            case UpgradeStatus.Spread:
                m_UpgradeStatus = UpgradeStatus.Burst;
                break;
        }
    }

    public void ActivateGameOver()
    {
        GameManager.Instance.ShowGameOver();  // If the player is hit by an enemy ship or laser it's game over.
        playerRenderer.enabled = false;       // We can't destroy the player game object straight away or any code from this point on will not be executed
        playerCollider.enabled = false;       // We turn off the players renderer so the player is not longer displayed and turn off the players collider
        playerThrust.Stop();
        Instantiate(explosion, transform.position, transform.rotation);   // Then we Instantiate the explosions... one at the centre and some additional around the players location for a bigger bang!
        for (int i = 0; i < 8; i++)
        {
            Vector3 randomOffset = new Vector3(transform.position.x + Random.Range(-0.6f, 0.6f), transform.position.y + Random.Range(-0.6f, 0.6f), 0.0f);
            Instantiate(explosion, randomOffset, transform.rotation);
        }
        Destroy(gameObject, 1.0f); // The second parameter in Destroy is a delay to make sure we have finished exploding before we remove the player from the scene.
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        m_Direction = callbackContext.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            switch (m_UpgradeStatus)
            {
                case UpgradeStatus.ShootOne:
                    ShootOne();
                    break;

                case UpgradeStatus.TripleShoot:
                    ShootTriple();
                    break;

                case UpgradeStatus.Spread:
                    ShootSpread();
                    break;

                case UpgradeStatus.Burst:
                    ShootBurst();
                    break;
            }
        }
    }

    public void TakeDamage(int _damage)
    {
        m_Health -= _damage;
        if (m_Health <= 0)
        {
            GameManager.Instance.ShowGameOver();
        }
    }
}