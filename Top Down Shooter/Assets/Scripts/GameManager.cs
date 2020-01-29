using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] Players;
    [SerializeField] private Camera m_Camera1;
    [SerializeField] private Camera m_Camera2;
    [SerializeField] private Camera m_Camera3;
    [SerializeField] private Camera m_Camera4;
    [SerializeField] private GameObject m_Player1Selection;
    [SerializeField] private GameObject m_Player2Selection;
    [SerializeField] private GameObject m_Door;
    [SerializeField] private Sprite m_OpenDoorSprite;
    private PlayerSelection m_Selection;
    public GameObject UiPlayer1;
    public GameObject UiPlayer2;
    public Vector3 ClickedPosition;

    private PlayerInputManager m_PlayerInputManager;

    private void Start()
    {
        m_Selection = FindObjectOfType<PlayerSelection>();
        Players = new GameObject[2];
        m_PlayerInputManager = GetComponent<PlayerInputManager>();
        m_Camera2.enabled = false;
        m_Camera3.enabled = false;
        m_Camera4.enabled = false;
        UiPlayer2.SetActive(false);
        if (m_Selection != null)
        {
            m_PlayerInputManager.playerPrefab = m_Selection.Player1Selection;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_PlayerInputManager.playerCount >= 2 && Players[1] != null)
        {
            PlayerInput pi = Players[1].GetComponent<PlayerInput>();

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

    public void OpenDoor()
    {
        SpriteRenderer spriteRenderer = m_Door.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = m_OpenDoorSprite;
        m_Door.GetComponent<Collider2D>().enabled = false;
    }

    private void PlayerChecker()
    {
        if (Players[0] != null)
        {
            PlayerInput p = Players[0].GetComponent<PlayerInput>();
            p.camera = m_Camera1;
            Player player = Players[0].GetComponent<Player>();
            player.SetCam(m_Camera1);
            Player o = Players[0].GetComponent<Player>();
            o.SetUi(UiPlayer1.GetComponent<UIPlayerHealth>());
            Debug.DrawLine(Players[0].transform.position, player.m_MousePostion + player.m_Offset);
        }
        if (Players[1] != null)
        {
            PlayerInput p = Players[1].GetComponent<PlayerInput>();
            p.camera = m_Camera2;
            Player player = Players[1].GetComponent<Player>();
            player.SetCam(m_Camera2);
            Player o = Players[1].GetComponent<Player>();
            o.SetUi(UiPlayer2.GetComponent<UIPlayerHealth>());
            UiPlayer2.SetActive(true);
            Debug.DrawLine(Players[1].transform.position, player.m_MousePostion + player.m_Offset);
        }
    }

    public int GetPlayerCount()
    {
        return m_PlayerInputManager.playerCount;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}