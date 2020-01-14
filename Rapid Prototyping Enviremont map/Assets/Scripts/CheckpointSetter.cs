using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSetter : MonoBehaviour
{
    private GameManager m_GameManager;

    private void Awake()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_GameManager.SetCheckPointPostion(transform.position);
    }
}