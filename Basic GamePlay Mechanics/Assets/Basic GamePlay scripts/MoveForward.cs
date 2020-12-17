using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
    }
}