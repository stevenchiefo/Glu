using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ServiceManager : MonoBehaviour
{
    public enum ServiceType
    {
        ServerHosting,
        CallService
    };

    public static ServiceManager Instance;

    public bool IsShown;

    private delegate void ServiceManagerTask();

    private FileOperator m_FileOperator;

    private string m_FilePath = "Services.json";

    [SerializeField] private Service[] m_Services;

    private Dictionary<ServiceType, ServiceData> m_ServicesData;

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
        m_FileOperator = new FileOperator(m_FilePath);
        ServiceManagerTask serviceManagerTask = new ServiceManagerTask(LoadFiles);
        IAsyncResult asyncResult = serviceManagerTask.BeginInvoke(null, null);
    }

    private void Start()
    {
        ShowOrHide(false);
        SetData();
    }

    public void ShowOrHide(bool boolean)
    {
        IsShown = boolean;
        for (int i = 0; i < m_Services.Length; i++)
        {
            m_Services[i].HideOrShow(boolean);
        }
    }

    public void StartSave()
    {
        ServiceManagerTask serviceManagerTask = new ServiceManagerTask(SaveFile);
        IAsyncResult asyncResult = serviceManagerTask.BeginInvoke(null, null);
    }

    private void SaveFile()
    {
        List<ServiceData> serviceDatas = new List<ServiceData>();
        for (int i = 0; i < m_Services.Length; i++)
        {
            serviceDatas.Add(m_Services[i].GetServiceData());
        }

        m_FileOperator.WriteFile(serviceDatas.ToArray());
    }

    private void LoadFiles()
    {
        //if (m_FileOperator.AlreadyExits())
        //{
        //    ServiceData[] serviceDatas = m_FileOperator.ReadFile<ServiceData[]>();

        //    if (serviceDatas.Length != m_Services.Length)
        //    {
        //        Debug.LogError("SavedServicesData is not equal to Current services");
        //    }

        //    for (int i = 0; i < m_Services.Length; i++)
        //    {
        //        m_Services[i].AssignData(serviceDatas[i]);
        //    }
        //}
        //else
        //{
        //    return;
        //}
    }

    private void OnApplicationQuit()
    {
        StartSave();
    }

    private void SetData()
    {
        m_ServicesData = new Dictionary<ServiceType, ServiceData>()
        {
            {ServiceType.CallService, new ServiceData()
            {
                Name = "Call Service",
                AmoutOfMoney = 130,
                Timer = 45,
                IsActive = false,
                RequiredLevel = 0,
            }
            },
            { ServiceType.ServerHosting, new ServiceData()
            {
                Name = "Server Hosting",
                AmoutOfMoney = 250,
                Timer = 60,
                IsActive = false,
                RequiredLevel = 1,
            }
            },
        };

        for (int i = 0; i < m_Services.Length; i++)
        {
            m_Services[i].AssignData(m_ServicesData[m_Services[i].m_ServiceType]);
        }
    }
}