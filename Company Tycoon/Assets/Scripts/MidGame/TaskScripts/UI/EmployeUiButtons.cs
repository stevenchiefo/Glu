using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeUiButtons : MonoBehaviour
{
    public static EmployeUiButtons Instance;

    public bool m_IsShown;
    [SerializeField] private Image[] m_Pages;
    [SerializeField] private Button[] m_Buttons;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this);
        }
    }

    public void Hide()
    {
        m_IsShown = false;
        for (int i = 0; i < m_Pages.Length; i++)
        {
            m_Pages[i].enabled = false;
        }
        for (int i = 0; i < m_Buttons.Length; i++)
        {
            m_Buttons[i].gameObject.SetActive(false);
        }
        EmployeManager.Instance.HideOrShowHiredEmployes(false);
        EmployeManager.Instance.HideOrShowHireEmployes(false);
    }

    public void Show()
    {
        m_IsShown = true;
        for (int i = 0; i < m_Pages.Length; i++)
        {
            m_Pages[i].enabled = true;
        }
        for (int i = 0; i < m_Buttons.Length; i++)
        {
            m_Buttons[i].gameObject.SetActive(true);
        }
        OnForHireClick();
    }

    public void OnForHireClick()
    {
        EmployeManager.Instance.HideOrShowHiredEmployes(false);
        EmployeManager.Instance.HideOrShowHireEmployes(true);
    }

    public void OnHiredClick()
    {
        EmployeManager.Instance.HideOrShowHiredEmployes(true);
        EmployeManager.Instance.HideOrShowHireEmployes(false);
    }
}