using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EmailConditionTypes
{
    Raise,
    Sick,
    Quiting,
    ToOld,
    IsGettingOld,
    GotHired,
    GotFired,
}

public class Email : MonoBehaviour
{
    private EmailData m_EmailData;
    private EmployeData m_EmpolyeData;

    public bool CanUse { get; set; }

    [SerializeField] private Text m_NameText;
    [SerializeField] private Text m_Subject;
    [SerializeField] private Text m_Message;
    [SerializeField] private Button m_AcceptButton;
    [SerializeField] private Button m_RefuseButton;
    [SerializeField] private GameObject m_ShowObj;

    /// <summary>
    /// Writes the data in the email
    /// </summary>
    /// <param name="emailData"></param>
    public void WriteEmail(EmailData emailData)
    {
        m_EmailData = emailData;
        m_EmpolyeData = emailData.EmployeData;
        m_NameText.text = m_EmailData.Name;
        m_Subject.text = m_EmailData.Subject;
        m_Message.text = m_EmailData.Message;
    }

    /// <summary>
    /// Show Object or not
    /// </summary>
    /// <param name="boolean"></param>
    public void HideOrShow(bool boolean)
    {
        m_ShowObj.SetActive(boolean);
    }

    /// <summary>
    /// On AcceptMail
    /// </summary>
    public void AcceptMail()
    {
        Employe employe = EmployeManager.Instance.GetEmploye(m_EmpolyeData);
        switch (m_EmailData.ConditionTypes)
        {
            case EmailConditionTypes.Raise:
                if (employe != null)
                {
                    m_EmpolyeData.Salary = m_EmailData.AmountToRaise;
                }

                break;

            case EmailConditionTypes.Sick:
                employe.SetSick(true);
                break;

            case EmailConditionTypes.Quiting:
                EmployeManager.Instance.FireEmploye(employe);
                break;

            case EmailConditionTypes.ToOld:
                EmployeManager.Instance.FireEmploye(employe);
                break;

            case EmailConditionTypes.IsGettingOld:

                break;

            case EmailConditionTypes.GotHired:
                break;

            case EmailConditionTypes.GotFired:
                break;
        }
        m_EmpolyeData.SendedEmail = false;
        employe.SetEmployeDayta(m_EmpolyeData);
        MailManager.Instance.ReturnEmail(this);
    }

    public void RefuseMail()
    {
        Employe employe = EmployeManager.Instance.GetEmploye(m_EmpolyeData);
        switch (m_EmailData.ConditionTypes)
        {
            case EmailConditionTypes.Raise:
                EmployeManager.Instance.FireEmploye(employe);

                break;

            case EmailConditionTypes.Sick:
                employe.SetSick(true);

                break;

            case EmailConditionTypes.Quiting:
                EmployeManager.Instance.FireEmploye(employe);
                break;

            case EmailConditionTypes.ToOld:
                EmployeManager.Instance.FireEmploye(employe);
                break;

            case EmailConditionTypes.IsGettingOld:
                break;

            case EmailConditionTypes.GotHired:
                break;

            case EmailConditionTypes.GotFired:
                break;
        }
        MailManager.Instance.ReturnEmail(this);
    }

    public EmailData GetEmailData()
    {
        return m_EmailData;
    }
}