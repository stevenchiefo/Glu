using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private GameObject m_MailTab;

    private void Start()
    {
        EmployeUiButtons.Instance.Hide();
    }

    public void ClickOnMail()
    {
        if (m_MailTab.activeSelf)
        {
            m_MailTab.SetActive(false);
            ServiceManager.Instance.ShowOrHide(false);
            EmployeUiButtons.Instance.Hide();
        }
        else
        {
            m_MailTab.SetActive(true);
            ServiceManager.Instance.ShowOrHide(false);
            EmployeUiButtons.Instance.Hide();
        }
    }

    public void ClickOnServicesTab()
    {
        if (ServiceManager.Instance.IsShown)
        {
            m_MailTab.SetActive(false);
            ServiceManager.Instance.ShowOrHide(false);
            EmployeUiButtons.Instance.Hide();
        }
        else
        {
            m_MailTab.SetActive(false);
            ServiceManager.Instance.ShowOrHide(true);
            EmployeUiButtons.Instance.Hide();
        }
    }

    public void ClickOnEmployeTab()
    {
        if (EmployeUiButtons.Instance.m_IsShown)
        {
            m_MailTab.SetActive(false);
            ServiceManager.Instance.ShowOrHide(false);
            EmployeUiButtons.Instance.Hide();
        }
        else
        {
            EmployeUiButtons.Instance.Show();
            m_MailTab.SetActive(false);
            ServiceManager.Instance.ShowOrHide(false);
        }
    }

    public void ClickOnQuit()
    {
        MailManager.Instance.StartSaveFile();
        EmployeManager.Instance.StartSave();
        Player.Instance.StartSave();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}