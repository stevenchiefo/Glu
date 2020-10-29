using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool GameOver;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
        GameOver = false;
    }

    public void SetGameOver()
    {
        GameOver = true;
        UIManager.Instance.ShowOrHideLevelFailed(true);
    }

    public void SetGameWon()
    {
        GameOver = true;
        UIManager.Instance.ShowOrHideLevelComplete(true);
    }
}
