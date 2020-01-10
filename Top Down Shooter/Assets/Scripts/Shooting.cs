using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject m_Bullit;

    // Update is called once per frame
    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        Instantiate(m_Bullit, transform.position, transform.rotation);
    }
}