using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    private Brain m_Brain;
    private Detection m_Detection;
    private Movement m_Movement;

    public bool Failed { get; private set; }
    public bool Finished { get; private set; }

    public Brain Brain { get { return m_Brain; } }

    private void Start()
    {
        LoadBrain();
        m_Detection = GetComponent<Detection>();
        m_Movement = GetComponent<Movement>();
    }

    private void Update()
    {
        float[] input = new float[]
        {
            m_Detection.FrontDetectionRayOutput,
            m_Detection.LeftDetectionRayOutput,
            m_Detection.RightDetectionRayOutput,
            1f,
        };
        float amount = m_Brain.FrontPerceptron.Geuss(input);
        amount += m_Brain.LeftPerceptron.Geuss(input);
        amount += m_Brain.RightPerceptron.Geuss(input);
        m_Movement.MoveTo(amount);
    }

    public void ReStart()
    {
        LoadBrain();
    }

    public void SetBrain(Brain brain)
    {
        m_Brain.FrontPerceptron = brain.CopyFrontPerceptron();
        m_Brain.LeftPerceptron = brain.CopyLeftPerceptron();
        m_Brain.RightPerceptron = brain.CopyRightPerceptron();
    }

    private void LoadBrain()
    {
        m_Brain = new Brain();
        m_Brain.FrontPerceptron = new Perceptron(4);
        m_Brain.LeftPerceptron = new Perceptron(4);
        m_Brain.RightPerceptron = new Perceptron(4);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CheckForWall(collision))
        {
            Failed = true;
        }
        else if (CheckForFinish(collision))
        {
            Finished = true;
        }
    }

    private bool CheckForWall(Collision collision)
    {
        return collision.gameObject.tag.ToLower() == "wall";
    }

    private bool CheckForFinish(Collision collision)
    {
        return collision.gameObject.tag.ToLower() == "finish";
    }
}