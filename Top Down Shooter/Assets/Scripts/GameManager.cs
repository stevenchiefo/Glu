using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject m_Player;

    private void Start()
    {
        LoadAll();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void LoadAll()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }
}