using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    // Make sure this class is a singleton
    //--------------------------------------------------------------------------------
    private static GameManager instance;

    public static GameManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(this);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    //--------------------------------------------------------------------------------
    // Class implementation
    //--------------------------------------------------------------------------------
    [Header("UI:")]
    public Text gameOverLabel;

    public Button restartGameButton;

    [Header("Enemy objects:")]
    public GameObject enemyType1;

    public GameObject enemyType2;

    [Header("Enemy config:")]
    public float startWait = 1.0f;

    public float waveInterval = 2.0f;
    public float spawnInterval = 0.5f;
    public int enemiesPerWave = 5;

    [SerializeField] private GameObject Canvas;

    [Header("Debug:")]
    public bool useObjectPool = true;

    private int m_Score;

    public void AddScore()
    {
        m_Score++;
    }

    public int GetScore()
    {
        return m_Score;
    }

    public void ShowGameOver()
    {
        Canvas.SetActive(true);
        gameOverLabel.rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
        restartGameButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -50, 0);
    }

    private void OnLevelWasLoaded(int level)
    {
        Canvas.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}