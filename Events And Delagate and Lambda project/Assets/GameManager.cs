using UnityEngine;
using TMPro;
[RequireComponent(typeof(Timer))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool LevelWon;

    private Timer m_Timer;
    private HitBoxSpawner m_HitBoxSpawner;
    [SerializeField] private int MaxClicks;

    [SerializeField] private GameObject m_UI;
    [SerializeField] private TextMeshProUGUI m_TimerText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        m_Timer = GetComponent<Timer>();
        m_HitBoxSpawner = GetComponent<HitBoxSpawner>();

        m_HitBoxSpawner.ClicksReached += LevelComplete;
        m_HitBoxSpawner.ClicksReached += m_Timer.Stop;

        m_Timer.StartTimer();
        m_HitBoxSpawner.Spawn(MaxClicks);
    }

    private void LevelComplete()
    {
        LevelWon = true;
        m_UI.SetActive(true);
        string context = $"Time:\n{m_Timer.Minutes}:{m_Timer.Seconds}:{m_Timer.Miliseconds}";
        m_TimerText.text = context;
    }

    public void OnRetry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
    
}
