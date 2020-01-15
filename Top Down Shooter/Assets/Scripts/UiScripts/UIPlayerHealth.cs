using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private Image[] m_Hearts = new Image[3];
    [SerializeField] private Health[] m_Healths = new Health[3];
    private string m_FilePathSprites = "HealthSprites";
    [SerializeField] private Sprite[] m_HealthSprites;

    private void Awake()
    {
        m_HealthSprites = Resources.LoadAll<Sprite>(m_FilePathSprites);
        GameObject[] o = GameObject.FindGameObjectsWithTag("Health");
        for (int i = 0; i < m_Hearts.Length; i++)
        {
            m_Hearts[i] = o[i].GetComponent<Image>();
            m_Healths[i] = new Health(m_HealthSprites[1]);
        }
    }

    private void Update()
    {
        SetImage();
        if (Input.GetKeyDown(KeyCode.E))
        {
            TookDamage();
        }
    }

    private void SetImage()
    {
        for (int i = 0; i < m_Hearts.Length; i++)
        {
            m_Hearts[i].sprite = m_Healths[i].HPIcon;
        }
    }

    public void TookDamage()
    {
        for (int i = m_Healths.Length - 1; i >= 0; i--)
        {
            if (m_Healths[i].Status != HpStatus.Empty)
            {
                if (m_Healths[i].Status == HpStatus.Full)
                {
                    m_Healths[i].HPIcon = m_HealthSprites[2];
                    m_Healths[i].Status = HpStatus.Half;
                    return;
                }
                if (m_Healths[i].Status == HpStatus.Half)
                {
                    m_Healths[i].HPIcon = m_HealthSprites[0];
                    m_Healths[i].Status = HpStatus.Empty;
                    return;
                }
            }
        }
    }
}

public enum HpStatus
{
    Full,
    Half,
    Empty
}

public class Health
{
    public Sprite HPIcon;
    public HpStatus Status = HpStatus.Full;

    public Health(Sprite image)
    {
        HPIcon = image;
    }
}