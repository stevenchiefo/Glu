using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public GameObject Player1Selection;

    public void SetPlayerSelection(GameObject gameObject)
    {
        Player1Selection = gameObject;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}