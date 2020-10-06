using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnNewStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnLoad()
    {
        SceneManager.LoadScene(2);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}