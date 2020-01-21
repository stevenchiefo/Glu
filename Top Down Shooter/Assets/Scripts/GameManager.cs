using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject m_Player1;
    public GameObject m_Player2;
    public GameObject m_Player3;
    public GameObject m_Player4;
    [SerializeField] private Camera m_Camera1;
    [SerializeField] private Camera m_Camera2;
    [SerializeField] private Camera m_Camera3;
    [SerializeField] private Camera m_Camera4;
    [SerializeField] private GameObject m_Player1Selection;
    [SerializeField] private GameObject m_Player2Selection;

    public GameObject UiPlayer1;
    public GameObject UiPlayer2;
    public Vector3 ClickedPosition;

    private PlayerInputManager m_PlayerInputManager;

    private void Start()
    {
        m_PlayerInputManager = GetComponent<PlayerInputManager>();
        m_Camera2.enabled = false;
        m_Camera3.enabled = false;
        m_Camera4.enabled = false;
        UiPlayer2.SetActive(false);
        m_PlayerInputManager.playerPrefab = m_Player1Selection;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_PlayerInputManager.playerCount >= 2 && m_Player2 != null)
        {
            PlayerInput pi = m_Player2.GetComponent<PlayerInput>();

            if (pi.camera != null)
            {
                m_Camera2.enabled = true;
                m_PlayerInputManager.splitScreen = true;
            }
        }
        if (m_PlayerInputManager.playerCount == 1)
        {
            m_PlayerInputManager.playerPrefab = m_Player2Selection;
        }
        PlayerChecker();
    }

    private void PlayerChecker()
    {
        if (m_Player1 != null)
        {
            PlayerInput p = m_Player1.GetComponent<PlayerInput>();
            p.camera = m_Camera1;
            Player player = m_Player1.GetComponent<Player>();
            player.SetCam(m_Camera1);
            Player o = m_Player1.GetComponent<Player>();
            o.SetUi(UiPlayer1.GetComponent<UIPlayerHealth>());
            Debug.DrawLine(m_Player1.transform.position, player.m_MousePostion + player.m_Offset);
        }
        if (m_Player2 != null)
        {
            PlayerInput p = m_Player2.GetComponent<PlayerInput>();
            p.camera = m_Camera2;
            Player player = m_Player2.GetComponent<Player>();
            player.SetCam(m_Camera2);
            Player o = m_Player2.GetComponent<Player>();
            o.SetUi(UiPlayer2.GetComponent<UIPlayerHealth>());
            UiPlayer2.SetActive(true);
            Debug.DrawLine(m_Player2.transform.position, player.m_MousePostion + player.m_Offset);
        }
        if (m_Player3 != null)
        {
            PlayerInput p = m_Player3.GetComponent<PlayerInput>();
            p.camera = m_Camera3;
            Player player = m_Player3.GetComponent<Player>();
            player.SetCam(m_Camera3);
            Debug.DrawLine(m_Player3.transform.position, player.m_MousePostion + player.m_Offset);
        }
        if (m_Player4 != null)
        {
            PlayerInput p = m_Player4.GetComponent<PlayerInput>();
            p.camera = m_Camera4;
            Player player = m_Player4.GetComponent<Player>();
            player.SetCam(m_Camera4);
            Debug.DrawLine(m_Player4.transform.position, player.m_MousePostion + player.m_Offset);
        }
    }

    public int GetPlayerCount()
    {
        return m_PlayerInputManager.playerCount;
    }
}