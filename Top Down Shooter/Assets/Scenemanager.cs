using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public void NextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void GoToMenu() => SceneManager.LoadScene(0);

    public void ExitGame() => Application.Quit();
}