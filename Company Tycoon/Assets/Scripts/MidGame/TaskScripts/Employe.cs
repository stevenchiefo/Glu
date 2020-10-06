using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Employe : MonoBehaviour
{
    [SerializeField] private int m_EmailCooldownCheck;
    [SerializeField] private float m_ChanceOFGettingSick;

    [SerializeField] private Text m_NameSpace;
    [SerializeField] private Text m_AgeSpace;
    [SerializeField] private Text m_SalarySpace;
    [SerializeField] private Image[] m_RatingStarts;
    [SerializeField] private Text m_HireOrFireText;
    [SerializeField] private GameObject m_ShowObj;

    [SerializeField] private Button m_LevelUpButton;
    [SerializeField] private Text m_ToLevelUpText;

    private delegate void EmployeTask();

    private bool m_CRisRunning;

    private EmployeData m_EmployeData;
    private bool m_IsSick;

    public void AssignEmployeData(EmployeData employeData)
    {
        m_EmployeData = employeData;
        m_LevelUpButton.interactable = true;
        SetUi();
    }

    public void HideOrShow(bool boolean)
    {
        m_ShowObj.SetActive(boolean);
    }

    private void Update()
    {
        CheckCoRoutine();
        CheckForLevelUp();
    }

    private void CheckCoRoutine()
    {
        if (gameObject.activeSelf)
        {
            if (m_CRisRunning == false)
            {
                StartCoroutine(CheckForEmail());
            }
        }
        else if (!gameObject.activeSelf)
        {
            StopCoroutine(CheckForEmail());
            m_CRisRunning = false;
        }
    }

    public void OnButton()
    {
        switch (m_EmployeData.Hired)
        {
            case true:
                RemoveEmploye();
                break;

            case false:
                HireEmploye();
                break;
        }
        HideOrShow(false);
    }

    public void HireEmploye()
    {
        EmployeManager.Instance.AddHireEmploye(this);
        if (!m_EmployeData.Hired)
        {
            SendHiredMail();
        }
        m_EmployeData.Hired = true;
        m_HireOrFireText.text = "Fire";
    }

    public void SetSick(bool boolean)
    {
        m_IsSick = boolean;
        SickTimer();
    }

    private IEnumerator SickTimer()
    {
        yield return new WaitForSeconds(60);
        m_IsSick = false;
    }

    public void SetEmployeDayta(EmployeData employeData)
    {
        m_EmployeData = employeData;
        SetUi();
    }

    public void SetUi()
    {
        SetStars();
        m_NameSpace.text = m_EmployeData.Name;
        m_AgeSpace.text = $"Age: {m_EmployeData.Age}";
        m_SalarySpace.text = $"Salary: {m_EmployeData.Salary}";
        m_ToLevelUpText.text = $"To Level Up: {m_EmployeData.AmountToLevelUp}$";
    }

    private void RemoveEmploye()
    {
        SendFireMail();
        EmployeManager.Instance.FireEmploye(this);
        m_EmployeData.Hired = false;
        m_HireOrFireText.text = "Hire";
    }

    private IEnumerator CheckForEmail()
    {
        while (true)
        {
            m_CRisRunning = true;
            if (m_EmployeData.Hired)
            {
                SendMail();
            }

            yield return new WaitForSeconds(m_EmailCooldownCheck);
        }
    }

    private void SetStars()
    {
        for (int i = 0; i < m_RatingStarts.Length; i++)
        {
            m_RatingStarts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < m_EmployeData.Rating; i++)
        {
            m_RatingStarts[i].gameObject.SetActive(true);
        }
    }

    private void SendHiredMail()
    {
        EmailData data = new EmailData
        {
            Name = m_EmployeData.Name,
            Subject = "Glad to work",
            Message = $"Hi Boss, I am new and I'm glad you hired me" +
            $"I will do my best for the business.\nThanks, {m_EmployeData.Name} ",
            ConditionTypes = EmailConditionTypes.GotHired,
            EmployeData = m_EmployeData,
        };

        m_EmployeData.SendedEmail = true;
        MailManager.Instance.SendMail(data);
    }

    private void SendFireMail()
    {
        EmailData data = new EmailData
        {
            Name = m_EmployeData.Name,
            Subject = "Got Fired",
            Message = $"Hi Ex boss, I dont know why you would fire me but i hope your business ends up in the trash.\nThanks, {m_EmployeData.Name} ",
            ConditionTypes = EmailConditionTypes.GotHired,
            EmployeData = m_EmployeData,
        };
        m_EmployeData.SendedEmail = true;
        MailManager.Instance.SendMail(data);
    }

    private void SendMail()
    {
        if (m_EmployeData.SendedEmail == false)
        {
            if (m_EmployeData.Age >= 64 && DoOrDoNot())
            {
                EmailData data = new EmailData
                {
                    Name = m_EmployeData.Name,
                    Subject = "Getting old",
                    Message = $"Hi boss, I am sorry to send you this \nBut i am getting old And i want to quit on my 65 and i am curnnetly {m_EmployeData.Age} So i hope my last year will be just as fun as the others.\nThanks, {m_EmployeData.Name}.",
                    ConditionTypes = EmailConditionTypes.IsGettingOld,
                    EmployeData = m_EmployeData,
                };
                m_EmployeData.SendedEmail = true;
                MailManager.Instance.SendMail(data);
            }
            else if (m_EmployeData.Age >= 65)
            {
                EmailData data = new EmailData
                {
                    Name = m_EmployeData.Name,
                    Subject = "To old",
                    Message = $"Hi boss, I am to old to work any longer I wish you a nice day hope we see each other again.\nThanks, {m_EmployeData.Name}.",
                    ConditionTypes = EmailConditionTypes.ToOld,
                    EmployeData = m_EmployeData,
                };
                m_EmployeData.SendedEmail = true;
                MailManager.Instance.SendMail(data);
                RemoveEmploye();
            }
            else if (m_EmployeData.YearsOfService > 2 && DoOrDoNot())
            {
                float multiplyer = UnityEngine.Random.Range(1f, 2f);
                int amountToRaise = Mathf.RoundToInt((float)m_EmployeData.Salary * multiplyer);
                EmailData data = new EmailData
                {
                    Name = m_EmployeData.Name,
                    Subject = "Want A Raise",
                    Message = $"Hi Boss. I have been working here for {m_EmployeData.YearsOfService} and i would like a raise if you would not mind, I have been working hard so I do think i deserve it i would like {amountToRaise} rather then {m_EmployeData.Salary} wich is my current salary\nThanks, {m_EmployeData.Name}.",
                    AmountToRaise = amountToRaise,
                    ConditionTypes = EmailConditionTypes.Raise,
                    EmployeData = m_EmployeData,
                };
                m_EmployeData.SendedEmail = true;
                MailManager.Instance.SendMail(data);
            }
            else if (m_EmployeData.YearsOfService > 5 && DoOrDoNot())
            {
                EmailData data = new EmailData
                {
                    Name = m_EmployeData.Name,
                    Subject = "Wants to go",
                    Message = $"Hi Boss. I have been thinking and i have been working here for {m_EmployeData.YearsOfService} and I think it has been enough i had fun working here but my time is come to leave Have a nice.\nThanks, {m_EmployeData.Name}.",
                    ConditionTypes = EmailConditionTypes.Quiting,
                    EmployeData = m_EmployeData,
                };
                m_EmployeData.SendedEmail = true;
                MailManager.Instance.SendMail(data);
            }
            else if (IsGettingSick())
            {
                EmailData data = new EmailData
                {
                    Name = m_EmployeData.Name,
                    Subject = "I am sick",
                    Message = $"Hi Boss. I am sick Today so I can't come to work sorry have a nice day.\nThanks, {m_EmployeData.Name}.",
                    ConditionTypes = EmailConditionTypes.Sick,
                    EmployeData = m_EmployeData,
                };
                m_EmployeData.SendedEmail = true;
                MailManager.Instance.SendMail(data);
            }
        }
    }

    private bool IsGettingSick()
    {
        int index = UnityEngine.Random.Range(0, 100);
        return index <= m_ChanceOFGettingSick;
    }

    private bool DoOrDoNot()
    {
        int index = UnityEngine.Random.Range(0, 100);
        int Precent = UnityEngine.Random.Range(0, 50);
        return index >= Precent;
    }

    public int GetId()
    {
        return m_EmployeData.ID;
    }

    public bool IsSick()
    {
        return m_IsSick;
    }

    public int GetWorkingSpeed()
    {
        return m_EmployeData.WorkingSpeed;
    }

    private void CheckForLevelUp()
    {
        if (m_EmployeData.Hired)
        {
            int ammountmoney = Player.Instance.GetMoney();
            m_LevelUpButton.interactable = ammountmoney >= m_EmployeData.AmountToLevelUp;
            return;
        }
        else
        {
            m_LevelUpButton.interactable = false;
        }
    }

    public EmployeData GetEmployeData()
    {
        return m_EmployeData;
    }

    public void LevelUp()
    {
        if (m_EmployeData.Rating == 5)
        {
            m_LevelUpButton.gameObject.SetActive(false);
            m_ToLevelUpText.gameObject.SetActive(false);
            return;
        }
        if (m_EmployeData.Rating != 5)
        {
            Player.Instance.AddXp(2f * m_EmployeData.Rating);
            Player.Instance.RemoveMoney(m_EmployeData.AmountToLevelUp);
            int oldworkingspeed = m_EmployeData.WorkingSpeed;
            oldworkingspeed = Mathf.RoundToInt(oldworkingspeed / (1f + (m_EmployeData.Rating / 10f)));
            if (m_EmployeData.Rating == 0)
            {
                oldworkingspeed = Mathf.RoundToInt(m_EmployeData.WorkingSpeed / 1f);
            }
            m_EmployeData.Rating += 1;
            m_EmployeData.WorkingSpeed = Mathf.RoundToInt(oldworkingspeed * (1f + (m_EmployeData.Rating / 10f)));

            m_EmployeData.AmountToLevelUp = Mathf.RoundToInt(m_EmployeData.AmountToLevelUp * (1f + (m_EmployeData.Rating / 10f)));

            EmployeTask employeTask = new EmployeTask(SetUi);
            IAsyncResult asyncResult = employeTask.BeginInvoke(null, null);
            if (m_EmployeData.Rating == 5)
            {
                m_LevelUpButton.gameObject.SetActive(false);
                m_ToLevelUpText.gameObject.SetActive(false);
            }
        }
    }
}