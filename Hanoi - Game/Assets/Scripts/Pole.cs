using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public Stack<GameObject> m_Dics = new Stack<GameObject>();
    private GameManager m_GameManager;
    [SerializeField] public string TopDisc;

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_Dics.Count > 0)
        {
            TopDisc = m_Dics.Peek().name;
        }
    }

    private void OnTriggerEnter(Collider info)
    {
        for (int i = 0; i < m_GameManager.m_Dics.Length; i++)
        {
            if (m_GameManager.m_Dics[i].m_Status == Disc.Status.Falling)
            {
                m_Dics.Push(info.gameObject);
                Debug.Log(info.gameObject.name + " Has been added to " + gameObject.name);
                print(gameObject.name + " does have " + m_Dics.Count + " discs");
            }
        }
    }

    private void OnTriggerExit(Collider info)
    {
        for (int i = 0; i < m_GameManager.m_Dics.Length; i++)
        {
            if (m_GameManager.m_Dics[i].m_Status == Disc.Status.Falling)
            {
                m_GameManager.m_Dics[i].m_Status = Disc.Status.Waiting_For_First_Click;
            }
            else if (m_GameManager.m_Dics[i].m_Status == Disc.Status.Move_To_Middle_Point)
            {
                print(m_Dics.Peek().name + " I have been removed from " + gameObject.name);
                m_Dics.Pop();
                print(gameObject.name + " does have " + m_Dics.Count + " discs");
            }
        }
    }

    private void OnMouseDown()
    {
        for (int i = 0; i < m_GameManager.m_Dics.Length; i++)
        {
            Disc disc = m_GameManager.m_Dics[i].GetComponent<Disc>();
            if (MayDiscBePlaced(disc.Num) == true)
            {
                if (m_GameManager.m_Dics[i].gameObject == m_GameManager.spot.m_Selected.gameObject && disc.m_Status == Disc.Status.Waiting_For_First_Click || m_GameManager.m_Dics[i].gameObject == m_GameManager.spot.m_Selected.gameObject && disc.m_Status == Disc.Status.Waiting_For_Second_Click)
                {
                    m_GameManager.m_Dics[i].EndPositionSetter(transform.position);
                }
            }
        }
    }

    private bool MayDiscBePlaced(int num)
    {
        for (int i = 0; i < m_GameManager.m_Dics.Length; i++)
        {
            if (m_Dics.Count >= 1)
            {
                Disc disc = m_Dics.Peek().GetComponent<Disc>();
                if (num < disc.Num)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}