using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    private PlayerInputManager m_PlayerInputManager;

    private void Awake()
    {
        m_PlayerInputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnEnable()
    {
        m_PlayerInputManager.enabled = true;
    }

    private void OnDisable()
    {
        m_PlayerInputManager.enabled = false;
    }
}