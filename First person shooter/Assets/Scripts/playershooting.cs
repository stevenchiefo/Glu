using UnityEngine;

public class playershooting : MonoBehaviour
{
    private GameObject m_Player;
    public GameObject bullit;
    private Vector3 m_Offset = new Vector3(0, 0, 1);
    private Camera m_Cam;
    public Ray raysatShot;
    private float m_Powerd = 1;
    private float m_Timer = 0;
    private bool m_Knockback = false;

    // Update is called once per frame
    private void Start()
    {
        m_Cam = FindObjectOfType<Camera>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        ShotClick();
        gameObject.transform.rotation = m_Cam.transform.rotation;
    }

    private void ShotClick()
    {
        m_Timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && m_Timer >= 1 / 120)
        {
            m_Timer = 0;
            if (m_Powerd <= 5)
            {
                m_Powerd += 0.01f;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Shooting();
        }
        
    }

    private void FixedUpdate()
    {
        if (m_Knockback == true)
        {
            m_Knockback = false;
            Rigidbody player = m_Player.GetComponent<Rigidbody>();
            player.AddRelativeForce(Vector3.back * (5 * m_Powerd), ForceMode.Impulse);
        }
    }

    private void Shooting()
    {
        m_Knockback = true;
        Vector3 postion = transform.position;
        GameObject b = bullit;
        Vector3 startScale = b.transform.localScale;
        b.transform.localScale = new Vector3(b.transform.localScale.x * m_Powerd, b.transform.localScale.y * m_Powerd, b.transform.localScale.z);
        bullittravel bt = b.GetComponent<bullittravel>();
        float dm = 20;
        bt.m_Damage = 20 * m_Powerd;
        Instantiate(b, postion, m_Cam.transform.rotation);
        ResetStats(startScale, dm);
    }

    private void ResetStats(Vector3 startscale, float Damage)
    {
        m_Powerd = 1;
        GameObject b = bullit;
        bullittravel bt = b.GetComponent<bullittravel>();
        bullit.transform.localScale = startscale;
        bt.m_Damage = Damage;
    }
}