using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;

    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverText;

    public void ShowGameOver()
    {
        gameOverText.SetActive(true);
    }

    private void Awake()
    {
        gameOverText.SetActive(false);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void UpdateInterFace()
    {
        EntityManager entityManager = EntityManager.instance;
        if (entityManager.Player != null)
        {
            // Onderstaand textveld wordt iedere frame geupdate, gebruik het Observer pattern om alleen de health te laten updaten wanneer deze veranderd
            healthText.text = string.Format("Health: {0}", entityManager.Player.Health); // Los de FindObjectOfType op met het Singleton Pattern

            // Onderstaand textveld wordt iedere frame geupdate, gebruik het Observer pattern om alleen de score te laten updaten wanneer deze veranderd
            scoreText.text = string.Format("Score: {0}", ScoreManager.instance.Score); // Los de FindObjectOfType op met het Singleton Pattern
        }
    }
}