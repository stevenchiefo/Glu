using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : Unit
{
    [Header("Settings")]
    public Transform m_Target;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NaviagateTo(m_Target.position);
        }
    }
}