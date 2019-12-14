using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    public enum Status
    {
        Waiting_For_First_Click = 0,
        Waiting_For_Second_Click,
        Move_To_Middle_Point,
        Move_To_End_Position,
        Falling
    }

    [SerializeField] public Status m_Status;
    private Vector3 m_StartLocation;
    private Vector3 m_EndLocation;

    public int Num = 0;
    private float m_TurnTimer;
    [SerializeField, Range(0.1f, 10f)] private float m_Speed;
    [SerializeField] public Rigidbody m_Body;
    private GameManager m_GameManager;

    public Disc()
    {
        m_Speed = 5;
    }

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        MoveToLocation();
    }

    public void EndPositionSetter(Vector3 pos)
    {
        m_EndLocation = new Vector3(m_GameManager.m_Middle.transform.position.x, m_GameManager.m_Middle.transform.position.y, pos.z);
        m_Status = Status.Move_To_End_Position;
    }

    public void MoveToLocation()
    {
        if (m_Status == Status.Move_To_Middle_Point)
        {
            m_Body.useGravity = false;
            m_TurnTimer += Time.deltaTime;

            m_StartLocation = gameObject.transform.position;
            float t = 0;
            t += m_TurnTimer * m_Speed;
            Vector3 pos = new Vector3(m_StartLocation.x, m_GameManager.m_Middle.transform.position.y, m_StartLocation.z);
            transform.position = Vector3.Lerp(m_StartLocation, pos, t);
            if (m_StartLocation == pos)
            {
                ResetTimers();
                m_Status = Status.Waiting_For_Second_Click;
                SetCurrentLocation();
            }
        }
        if (m_Status == Status.Move_To_End_Position)
        {
            m_Body.useGravity = false;
            m_TurnTimer += Time.deltaTime;
            m_StartLocation = gameObject.transform.position;
            float t = 0;
            if (m_StartLocation == m_EndLocation)
            {
                t = 1;
            }
            t += m_TurnTimer * m_Speed;
            transform.position = Vector3.Lerp(m_StartLocation, m_EndLocation, t);
            if (m_StartLocation == m_EndLocation)
            {
                ResetTimers();
                m_Status = Status.Falling;
                m_Body.useGravity = true;
            }
        }
    }

    private void ResetTimers()
    {
        m_TurnTimer = 0;
    }

    private void SetCurrentLocation()
    {
        gameObject.transform.position = transform.position;
    }

    private void OnMouseDown()
    {
        Pole pole0 = m_GameManager.m_Pole0.GetComponent<Pole>();
        Pole pole1 = m_GameManager.m_Pole1.GetComponent<Pole>();
        Pole pole2 = m_GameManager.m_Pole2.GetComponent<Pole>();
        if (AllowdTeBeSelected() == true)
        {
            if (gameObject.name == pole0.TopDisc || gameObject.name == pole1.TopDisc || gameObject.name == pole2.TopDisc)
            {
                switch (m_Status)
                {
                    case Status.Waiting_For_First_Click:
                        m_Status = Status.Move_To_Middle_Point;
                        break;
                }
            }
        }
    }

    private bool AllowdTeBeSelected()
    {
        for (int i = 0; i < m_GameManager.m_Dics.Length; i++)
        {
            if (m_GameManager.m_Dics[i].m_Status == Status.Waiting_For_First_Click)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}