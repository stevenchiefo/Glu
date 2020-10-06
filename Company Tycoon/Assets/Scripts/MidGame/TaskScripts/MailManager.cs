using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MailManager : MonoBehaviour
{
    [SerializeField] private bool m_Load;

    public static MailManager Instance;

    private List<Email> m_Emails;
    private List<Email> m_ObjectPoolEmails;
    private FileOperator m_FileOperator;
    private string m_FilePath = "Emails.json";

    private delegate void MailOperation();

    [SerializeField] private Transform m_MailTab;
    [SerializeField] private GameObject m_EmailPrefab;
    [SerializeField] private GameObject m_NotifyPoint;
    [SerializeField] private Text m_NotifyText;
    [SerializeField] private int m_PoolSpawnRange;

    private void Awake()
    {
        m_Emails = new List<Email>();
        MakeEmailPool();
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        LoadFiles();
        UpdateEmailNotify();
    }

    private void Update()
    {
        OnMailON();
    }

    /// <summary>
    /// Notify Ui Updater
    /// </summary>
    private void UpdateEmailNotify()
    {
        if (m_Emails.Count > 0)
        {
            m_NotifyText.text = m_Emails.Count.ToString();
            if (!m_NotifyPoint.activeSelf)
            {
                m_NotifyPoint.SetActive(true);
            }
        }
        else
        {
            m_NotifyText.text = "";
            if (m_NotifyPoint.activeSelf)
            {
                m_NotifyPoint.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Adds email to the list
    /// </summary>
    /// <param name="email"></param>
    private void AddEmail(Email email)
    {
        m_Emails.Add(email);
        email.transform.position = m_MailTab.position;
        UpdateEmailNotify();
    }

    /// <summary>
    /// Get a nonActive email
    /// </summary>
    /// <returns></returns>
    private Email GetEmail()
    {
        if (m_ObjectPoolEmails != null)
        {
            for (int i = 0; i < m_ObjectPoolEmails.Count; i++)
            {
                if (m_ObjectPoolEmails[i].CanUse)
                {
                    m_ObjectPoolEmails[i].CanUse = false;
                    return m_ObjectPoolEmails[i];
                }
            }
            GameObject _obj = Instantiate(m_EmailPrefab, transform);
            Email email = _obj.GetComponent<Email>();
            email.CanUse = false;
            m_ObjectPoolEmails.Add(email);
            return email;
        }
        return null;
    }

    /// <summary>
    /// Make the EmailPool
    /// </summary>
    private void MakeEmailPool()
    {
        m_ObjectPoolEmails = new List<Email>();
        for (int i = 0; i < m_PoolSpawnRange; i++)
        {
            GameObject _obj = Instantiate(m_EmailPrefab);
            _obj.transform.SetParent(transform);
            Email email = _obj.GetComponent<Email>();
            email.CanUse = true;
            _obj.SetActive(false);
            m_ObjectPoolEmails.Add(email);
        }
    }

    private void OnApplicationQuit()
    {
        StartSaveFile();
    }

    private void LoadFiles()
    {
        m_FileOperator = new FileOperator(m_FilePath);
        if (m_FileOperator.AlreadyExits() && m_Load == true)
        {
            LoadSavedEmails();
        }
    }

    /// <summary>
    /// Reads the json file to a emails
    /// </summary>
    private void LoadSavedEmails()
    {
        MailMangerSaveData mailMangerSaveData = new MailMangerSaveData();
        mailMangerSaveData = m_FileOperator.ReadFile<MailMangerSaveData>();
        for (int i = 0; i < mailMangerSaveData.EmailDatas.Length; i++)
        {
            SendMail(mailMangerSaveData.EmailDatas[i]);
        }

        UpdateEmailNotify();
    }

    /// <summary>
    /// Saves the email is a json file
    /// </summary>
    private void SaveFiles()
    {
        MailMangerSaveData mailMangerSaveData = new MailMangerSaveData();
        mailMangerSaveData.EmailDatas = new EmailData[m_Emails.Count];
        for (int i = 0; i < m_Emails.Count; i++)
        {
            mailMangerSaveData.EmailDatas[i] = m_Emails[i].GetEmailData();
        }

        m_FileOperator.WriteFile(mailMangerSaveData);
    }

    public void StartSaveFile()
    {
        MailOperation mailOperation = new MailOperation(SaveFiles);
        IAsyncResult asyncResult = mailOperation.BeginInvoke(null, null);
        asyncResult.AsyncWaitHandle.WaitOne();
    }

    /// <summary>
    /// Send A mail To the main email Storage
    /// </summary>
    /// <param name="emailData"></param>
    public void SendMail(EmailData emailData)
    {
        Email email = GetEmail();
        email.WriteEmail(emailData);
        email.transform.SetParent(m_MailTab);
        AddEmail(email);
    }

    /// <summary>
    /// Remove a email from the main storage
    /// </summary>
    /// <param name="email"></param>
    public void ReturnEmail(Email email)
    {
        m_Emails.Remove(email);
        email.CanUse = true;
        email.HideOrShow(false);
        email.gameObject.SetActive(false);
        UpdateEmailNotify();
    }

    public void OnMailON()
    {
        if (m_Emails.Count != 0)
        {
            m_Emails[0].gameObject.SetActive(true);
            m_Emails[0].HideOrShow(true);
        }
    }
}

[Serializable]
public struct MailMangerSaveData
{
    public EmailData[] EmailDatas;
}