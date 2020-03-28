using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirButton : MonoBehaviour
{
    [SerializeField] private AirCannon m_AirCannon;

    private void OnTriggerEnter(Collider other)
    {
        m_AirCannon.Shoot();
    }
}