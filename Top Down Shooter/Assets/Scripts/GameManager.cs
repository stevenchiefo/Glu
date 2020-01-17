using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject m_Player;
    public Vector3 ClickedPosition;
    public Vector3 MousePostion;

    private void Start()
    {
        LoadAll();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_Player == null)
        {
            m_Player = GameObject.FindGameObjectWithTag("Player");
        }

        MousePositionSetter();
        if (m_Player != null)
            Debug.DrawLine(m_Player.transform.position, MousePostion);
    }

    private void MousePositionSetter()
    {
        MousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClickedPosition = MousePostion;
        }
    }

    private void LoadAll()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }
}