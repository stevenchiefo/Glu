using UnityEngine;
using TMPro;

public class HitBoxSpawner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ClickCounter;
    [SerializeField] private GameObject m_HitBox;
    private RectTransform m_RectTransform;
    private int m_MaxClicks;
    private int m_CurrentClicks;
    private HitBox[] m_HitBoxes;

    [SerializeField] private Color[] m_Colors;

    public delegate void OnMaxClicksReached();
    public event OnMaxClicksReached ClicksReached;

    private void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    private void RespawnHitboxSpawner(HitBox hitBox)
    {
        if (m_CurrentClicks < m_MaxClicks)
        {
            m_CurrentClicks++;
            string context = $"{m_CurrentClicks}/{m_MaxClicks}";
            m_ClickCounter.text = context;
            SetTarget(hitBox);
        }
        if (m_CurrentClicks >= m_MaxClicks)
        {
            ClicksReached();
        }

    }

    private void SetColor(HitBox hitBox)
    {
        if (!GameManager.Instance.LevelWon)
        {


            int index = m_Colors.Length - 1;
            if (hitBox.ColorIndex < m_Colors.Length)
            {
                index = hitBox.ColorIndex;
                hitBox.ColorIndex++;
            }
            hitBox.m_Image.color = m_Colors[index];
            hitBox.DoTime = true;
            SetTarget(hitBox);
        }

    }

    private void SetTarget(HitBox hitBox)
    {
        if (GameManager.Instance.LevelWon == false)
        {
            float _x = Random.Range((-m_RectTransform.rect.width / 2) + hitBox.m_Rectransform.rect.width, (m_RectTransform.rect.width / 2) + -hitBox.m_Rectransform.rect.width);
            float _y = Random.Range((-m_RectTransform.rect.height / 2) + hitBox.m_Rectransform.rect.height, (m_RectTransform.rect.height / 2) + -hitBox.m_Rectransform.rect.height);
            hitBox.SetPosition(new Vector2(_x, _y));
        }
    }

    public void Spawn(int MaxClicks)
    {
        m_MaxClicks = MaxClicks;
        m_HitBoxes = new HitBox[1];
        m_CurrentClicks = -1;
        for (int i = 0; i < m_HitBoxes.Length; i++)
        {
            m_HitBoxes[i] = Instantiate(m_HitBox, transform).GetComponent<HitBox>();
            m_HitBoxes[i].OnPressed += RespawnHitboxSpawner;
            m_HitBoxes[i].OnMoveCoolDownComplete += SetColor;
            RespawnHitboxSpawner(m_HitBoxes[i]);
            StartCoroutine(m_HitBoxes[i].PrivateTimer(Random.Range(4f, 8f)));
        }

    }
}
