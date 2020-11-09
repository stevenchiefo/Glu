using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody m_Rigidbody;

    private void FixedUpdate()
    {
        m_Rigidbody.AddForceAtPosition(Physics.gravity, transform.position, ForceMode.Acceleration);
        float WaveHeight = WaveManager.Instance.WaveHeight(m_Rigidbody.position);
        if (m_Rigidbody.position.y < WaveHeight)
        {
            float _Power = WaveHeight - transform.position.y;
            m_Rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * WaveManager.Instance.GetFloatPower() * (1 + _Power / 10f), 0f) * Time.deltaTime, transform.position, ForceMode.Acceleration);
        }
    }
}