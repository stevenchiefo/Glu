using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Service : MonoBehaviour
{
    public ServiceManager.ServiceType m_ServiceType;

    [SerializeField] private GameObject m_ShowObj;
    [SerializeField] private Button m_Button;
    [SerializeField] private Text m_TimerText;
    [SerializeField] private Text m_ServiceTimerText;

    private int m_TimerTextKeeperSeconds;

    private ServiceData m_ServiceData;
    private List<Employe> m_AssignedEmployes;

    private delegate void ServiceTask();

    private int m_Timer;
    private float m_TimerSecond;

    private void Start()
    {
        Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
        buttonClickedEvent.AddListener(AssignService);
        m_Button.onClick = buttonClickedEvent;
        m_AssignedEmployes = new List<Employe>();
    }

    private void Update()
    {
        CheckIfCanAssign();
        KeepTimer();
        ServiceTask serviceTask = new ServiceTask(SetServiceTimerText);
        serviceTask.BeginInvoke(null, null);
    }

    public void AssignData(ServiceData serviceData)
    {
        m_ServiceData = serviceData;
    }

    public void StartService()
    {
        ServiceTask serviceTask = new ServiceTask(SetTimer);
        serviceTask.BeginInvoke(null, null);

        m_Button.gameObject.SetActive(false);
        m_TimerText.gameObject.SetActive(true);
        StartCoroutine(CheckForService());
    }

    private void SetTimer()
    {
        int WaitTime = m_ServiceData.Timer;
        Employe[] employes = EmployeManager.Instance.GetHiredEmployes();
        for (int i = 0; i < employes.Length; i++)
        {
            if (employes[i].IsSick() == false)
            {
                WaitTime = WaitTime - employes[i].GetWorkingSpeed();
            }
        }
        m_Timer = WaitTime;
        m_TimerTextKeeperSeconds = WaitTime;
    }

    private void SetServiceTimerText()
    {
        int AmmountToTakeOff = 0;
        Employe[] employes = EmployeManager.Instance.GetHiredEmployes();
        for (int i = 0; i < employes.Length; i++)
        {
            if (employes[i].IsSick() == false)
            {
                AmmountToTakeOff = AmmountToTakeOff + employes[i].GetWorkingSpeed();
            }
        }
        if (AmmountToTakeOff == 0)
        {
            m_ServiceTimerText.text = $"{m_ServiceData.Timer} seconds";
            return;
        }
        m_ServiceTimerText.text = $"{m_ServiceData.Timer} - {AmmountToTakeOff} seconds";
    }

    private void KeepTimer()
    {
        if (m_ServiceData.IsActive)
        {
            m_TimerSecond += Time.deltaTime;
            if (m_TimerSecond > 1f)
            {
                m_TimerSecond = 0f;
                m_TimerTextKeeperSeconds--;
            }
            m_TimerText.text = $"{m_TimerTextKeeperSeconds}";
        }
    }

    public void HideOrShow(bool boolean)
    {
        m_ShowObj.SetActive(boolean);
    }

    public void AssignService()
    {
        m_Button.interactable = false;
        m_Button.GetComponentInChildren<Text>().text = "Assigned";
        m_ServiceData.IsActive = true;
        StartService();
    }

    private void CheckIfCanAssign()
    {
        if (!m_ServiceData.IsActive)
        {
            if (Player.Instance != null)
            {
                m_Button.interactable = Player.Instance.GetPlayerData().Level >= m_ServiceData.RequiredLevel;
            }
        }
    }

    private IEnumerator CheckForService()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_Timer);
            PayService();
        }
    }

    public ServiceData GetServiceData()
    {
        return m_ServiceData;
    }

    private void PayService()
    {
        StopCoroutine(CheckForService());

        m_Button.interactable = true;
        m_Button.GetComponentInChildren<Text>().text = "Assign";
        m_ServiceData.IsActive = false;

        m_Button.gameObject.SetActive(true);
        m_TimerText.gameObject.SetActive(false);
        Player.Instance.AddMoney(m_ServiceData.AmoutOfMoney);
        Player.Instance.AddXp(1);

        EmployeManager.Instance.AgeEmployes();
    }
}