using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    private Brain m_Brain;
    private Detection m_Detection;
    private Movement m_Movement;
    private Vector3 m_LastPos;

    public bool Failed { get; private set; }
    public bool Finished { get; private set; }

    public Brain Brain { get { return m_Brain; } }

    private void Start()
    {
        LoadBrain();
        m_Detection = GetComponent<Detection>();
        m_Movement = GetComponent<Movement>();
        StartCoroutine(Timer());
    }

    private void Update()
    {
        float[] input = new float[m_Detection.Outputs.Length + 1];
        for (int i = 0; i < m_Detection.Outputs.Length; i++)
        {
            input[i] = m_Detection.Outputs[i];
        }
        input[input.Length - 1] = 1f;
        float amount = 0f;
        foreach (Perceptron i in m_Brain.Perceptrons)
        {
            amount += i.Geuss(input);
        }
        m_Movement.MoveTo(amount);
    }

    private IEnumerator Timer()
    {
        while (Failed == false && Finished == false)
        {
            yield return new WaitForSeconds(3f);
            if (CheckIfStuck())
            {
                Failed = true;
            }
        }
    }

    private bool CheckIfStuck()
    {
        float distance = Vector3.Distance(transform.position, m_LastPos);
        m_LastPos = transform.position;
        if (distance <= 3f)
        {
            return true;
        }
        return false;
    }

    public void ReStart()
    {
        LoadBrain();
    }

    public void SetBrain(Brain brain)
    {
        m_Brain.Perceptrons = brain.Copy();
    }

    private void LoadBrain()
    {
        m_Brain = new Brain();
        m_Brain.CreatePerceptrons(11, 12);
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