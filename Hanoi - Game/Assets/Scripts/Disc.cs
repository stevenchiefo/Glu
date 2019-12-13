using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    private enum Status
    {
        Waiting_For_First_Click = 0,
        Waiting_For_Second_Click,
        Move_To_Middle_Point,
        Move_To_End_Position,
    }

    [SerializeField] private Status m_Status;
    private Vector3 m_StartLocation;
    private Vector3 m_EndLocation;
    private Vector3 m_Position;
    private float m_TurnTimer;
    [SerializeField, Range(0.1f, 10f)] private float m_Speed;
    [SerializeField] public Rigidbody m_Body;

    public Disc()
    {
        m_Speed = 5;
    }

    private void Start()
    {
        m_Body.useGravity = false;
    }

    // Update is called once per frame
    private void Update()
    {
        MoveToLocation();
    }

    public void SetPosition(Vector3 endPos)
    {
        m_EndLocation = endPos;
        m_Status = Status.Move_To_Middle_Point;
    }

    public void MoveToLocation()
    {
        if (m_Status == Status.Move_To_Middle_Point)
        {
            m_TurnTimer += Time.deltaTime;
            GameObject middle = GameObject.Find("MidLocation");
            m_StartLocation = gameObject.transform.position;
            float t = 0;
            t += m_TurnTimer * m_Speed;
            transform.position = Vector3.Lerp(m_StartLocation, middle.transform.position, t);
            print(t.ToString());
            if (t >= 1)
            {
                ResetTimers();
                m_Status = Status.Waiting_For_First_Click;
                SetCurrentLocation();
                m_Body.useGravity = true;
            }
        }
        if (m_Status == Status.Move_To_End_Position)
        {
            m_TurnTimer += Time.deltaTime;
            m_StartLocation = gameObject.transform.position;
            float t = 0;
            t += m_TurnTimer * m_Speed;
            transform.position = Vector3.Lerp(m_StartLocation, m_EndLocation, t);
            if (t >= 1)
            {
                ResetTimers();
                m_Status = Status.Waiting_For_First_Click;
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
}