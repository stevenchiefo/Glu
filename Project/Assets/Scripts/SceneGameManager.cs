using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneGameManager : MonoBehaviour
{
    public static SceneGameManager instance;
    public bool GameOver { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void SetGameOver()
    {
        GameOver = true;
        FindObjectOfType<InterfaceManager>().ShowGameOver(); // Los de FindObjectOfType op met het Singleton Pattern
    }

    public void RestartButton(InputAction.CallbackContext context)
    {
        if (context.performed && GameOver)
        {
            SceneManager.LoadScene(0);
        }
    }
}