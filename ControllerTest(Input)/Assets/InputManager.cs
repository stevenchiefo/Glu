using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputManager m_PlayerInputManager;
    private PlayerInput m_Player;
    private InputDevice m_Gamepads;
    private bool m_BeenSwitched = false;
    private bool m_AlreadySpawned = false;

    private void Awake()
    {
        m_PlayerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        SwitchPerfab();
    }

    private void SwitchPerfab()
    {
        if (m_PlayerInputManager.playerCount == 1)
        {
            GameObject[] i = Resources.LoadAll<GameObject>("Player2");
            m_PlayerInputManager.playerPrefab = i[0];
            m_AlreadySpawned = true;
        }
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