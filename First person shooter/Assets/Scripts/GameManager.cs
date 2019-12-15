using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject[] m_Enemys = new GameObject[5];

    private void Start()
    {
        LoadEnemy();
    }

    // Update is called once per frame
    private void Update()
    {
        IsEnemyDefeated();
    }

    private void LoadEnemy()
    {
        for (int i = 0; i < m_Enemys.Length; i++)
        {
            m_Enemys[i] = GameObject.Find("Enemy" + i);
        }
    }

    private void IsEnemyDefeated()
    {
        for (int i = 0; i < m_Enemys.Length; i++)
        {
            if (m_Enemys[i] != null)
            {
                return;
            }
        }
        SceneLoader();
    }

    private void SceneLoader()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}