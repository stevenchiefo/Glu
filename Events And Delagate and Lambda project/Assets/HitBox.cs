using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitBox : MonoBehaviour
{
    

    public delegate void HitboxDelegate(HitBox hitBox);

    public event HitboxDelegate OnPressed;
    public event HitboxDelegate OnMoveCoolDownComplete;

    public Image m_Image;
    public RectTransform m_Rectransform;
    private Vector2 m_TargetPosition;
    private bool m_Move;

    private float m_TimerTarget;
    private float m_CurrentTimer;
    public int ColorIndex;
    public bool DoTime;

    private void Start()
    {
        m_Rectransform = GetComponent<RectTransform>();
        m_Image = GetComponent<Image>();
        StartCoroutine(MoveToTarget());
    }

    public bool GotHit
    {
        get
        {
            return m_GotHit;
        }
        set
        {
            if (value == true)
            {
                OnPressed(this);
            }
            m_GotHit = value;
        }
    }

    private bool m_GotHit;

    public IEnumerator PrivateTimer(float timer)
    {
        m_TimerTarget = timer;
        bool boolean = true;
        DoTime = true;
        while (boolean == true)
        {
            if (GameManager.Instance.LevelWon)
            {
                StopAllCoroutines();
            }
            yield return new WaitUntil(() => DoTime);
            yield return new WaitForSeconds(1);
            m_CurrentTimer += 1;
            if(m_CurrentTimer >= m_TimerTarget)
            {
                m_CurrentTimer = 0f;
                OnMoveCoolDownComplete(this);
                m_TimerTarget = m_TimerTarget * GetPercentage();
            }
            
        }
    }

    private float GetPercentage()
    {
        float ammount = 1f - (ColorIndex / 10f);
        return ammount;

    }

    private IEnumerator MoveToTarget()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_Move);
            m_Rectransform.localPosition = Vector2.Lerp(m_Rectransform.localPosition, m_TargetPosition, 0.2f);
            float distance = Vector2.Distance(m_Rectransform.localPosition, m_TargetPosition);
            if (distance < 1.5)
            {
                m_Rectransform.localPosition = m_TargetPosition;
                m_Move = false;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    public void SetPosition(Vector2 pos)
    {
        m_TargetPosition = pos;
        m_Move = true;
    }

    public void DeActive()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public void OnButton()
    {
        GotHit = true;
    }
}
