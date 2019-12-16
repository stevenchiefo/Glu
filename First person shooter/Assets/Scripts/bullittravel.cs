using UnityEngine;

public class bullittravel : MonoBehaviour
{
    private int m_BullitDistance = 0;
    [SerializeField, Range(0, 200)] public float FlySpeed;
    private ParticleSystem m_Explosion;
    public float m_Damage = 20;
    private bool m_Death = false;

    private void Awake()
    {
        m_Explosion = gameObject.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        BullitTravel();
    }

    private void BullitTravel()
    {
        if (m_BullitDistance >= 100)
        {
            DestroyBullit();
        }

        if (m_Death == false)
        {
            transform.Translate(Vector3.forward * FlySpeed * Time.deltaTime);
        }
        m_BullitDistance++;
    }

    private void OnCollisionEnter(Collision info)
    {
        if (info.gameObject.tag == "Enemy")
        {
            info.gameObject.GetComponent<Enemy>().GotHit(m_Damage);
            print("You hitted " + info.gameObject.name);
            DestroyBullit();
        }
        if (info.gameObject.tag == "Wall")
        {
            DestroyBullit();
        }
        else
        {
            DestroyBullit();
        }
    }

    private void DestroyBullit()
    {
        m_Death = true;
        ExplodeAnimation();
        Explode();
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.enabled = false;
        Invoke("DestoryObject", 0.5f);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, 5);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy e = hit.GetComponent<Enemy>();
                e.GotHit(ExplosionGen() / 3);
            }
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.tag != "bullit")
            {
                rb.AddExplosionForce(ExplosionGen(), gameObject.transform.position, 5, 0.5f, ForceMode.Impulse);
            }
        }
    }

    private float ExplosionGen()
    {
        float ExplosionForce = 20 * (m_Damage / 10);
        return ExplosionForce;
    }

    private void ExplodeAnimation()
    {
        m_Explosion.Play();
    }

    private void DestoryObject()
    {
        Destroy(gameObject);
    }
}