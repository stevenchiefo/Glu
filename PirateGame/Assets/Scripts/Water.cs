using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float m_MaxFloatPower;
    private float m_FloatPower;
    private bool m_Addup;

    private void Start()
    {
        StartCoroutine(CheckFloatPower());
        m_Addup = true;
    }

    private IEnumerator CheckFloatPower()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (m_Addup)
            {
                m_FloatPower += 10f;
                if (m_FloatPower >= m_MaxFloatPower)
                {
                    m_Addup = false;
                }
            }
            else
            {
                m_FloatPower += -10f;
                if (m_FloatPower <= 0)
                {
                    m_Addup = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CheckForBoat(other);
        CheckForTreasueChest(other);
    }

    private void CheckForBoat(Collider collider)
    {
        IShip ship = collider.GetComponentInParent<IShip>();
        if (ship != null)
        {
            Rigidbody _rb = ship.GetRB();
            Vector3 _Dir = new Vector3(0f, 30f, 0f);
            _rb.AddForce(_Dir * m_FloatPower * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    private void CheckForTreasueChest(Collider collider)
    {
        TreasureChest treasureChest = collider.GetComponentInParent<TreasureChest>();
        if (treasureChest != null)
        {
            Rigidbody _rb = treasureChest.GetRigidbody();
            Vector3 _Dir = new Vector3(0f, 30f, 0f);
            _rb.AddForce(_Dir * m_FloatPower * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}