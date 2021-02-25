using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    private Brain m_Brain;
    private Detection m_Detection;
    private Movement m_Movement;

    public bool Failed { get; private set; }
    public float Distance { get; private set; }

    private void Start()
    {
        LoadBrain();
        m_Detection = GetComponent<Detection>();
        m_Movement = GetComponent<Movement>();
    }

    private void Update()
    {
        m_Movement.MoveTo(m_Brain.Perceptron.Geuss(new float[] { m_Detection.LeftDetectionRayOutput, m_Detection.RightDetectionRayOutput, m_Detection.FrontDetectionRayOutput, 1 }));
        Distance += Time.deltaTime;
    }

    public void ReStart()
    {
        LoadBrain();
    }



    private void LoadBrain()
    {
        m_Brain = new Brain();
        m_Brain.Perceptron = new Perceptron(3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CheckForWall(collision))
        {
            Failed = true;
        }
    }

    private bool CheckForWall(Collision collision)
    {
        return collision.gameObject.tag.ToLower() == "wall";
    }
}
