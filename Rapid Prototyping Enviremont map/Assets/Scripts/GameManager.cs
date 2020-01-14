using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector3 m_CheckPointPositon;
    [SerializeField] private GameObject m_Player = null;
    private Vector3 m_Offset = new Vector3(0, 4, 0);

    private void Start()
    {
        m_CheckPointPositon = m_Player.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SetCheckPointPostion(Vector3 newpos)
    {
        m_CheckPointPositon = newpos;
    }

    public void SpawnAtCheckPoint()
    {
        //m_Player.GetComponent<FirstPersonController>().enabled = false;
        m_Player.transform.position = m_CheckPointPositon;
        //Invoke("TurnOnFirstPersonController", 0.1f);
    }

    private void TurnOnFirstPersonController()
    {
        m_Player.GetComponent<FirstPersonController>().enabled = true;
    }
}