using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmployeManager : MonoBehaviour
{
    [SerializeField] private bool m_Load;

    public static EmployeManager Instance;

    private List<Employe> m_HiredEmployes;
    private List<Employe> m_CanHireEmployes;
    private int m_Id;
    private FileOperator m_FileOperator;
    private string m_FilePath = "Employes.json";

    private delegate void EmployeManagerTask();

    [SerializeField] private float m_EmployeRefreshCooldown;
    [SerializeField] private Transform m_Tab;
    [SerializeField] private Transform m_InHireTab;
    [SerializeField] private GameObject m_EmployePrefab;
    [SerializeField] private int m_AtleastAmountOfEmp;

    private string[] m_Names;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        m_FileOperator = new FileOperator(m_FilePath);
        m_CanHireEmployes = new List<Employe>();
    }

    private void Start()
    {
        SetNames();
        LoadEmployes();
    }

    private void LoadEmployes()
    {
        m_HiredEmployes = new List<Employe>();
        if (m_FileOperator.AlreadyExits() && m_Load == true)
        {
            EmployeManagerSaveData employeManagerSaveData = new EmployeManagerSaveData();
            employeManagerSaveData = m_FileOperator.ReadFile<EmployeManagerSaveData>();
            m_Id = employeManagerSaveData.IDcount;
            SpawnFirst(employeManagerSaveData.Employedatas.Length);

            List<EmployeData> employeDatas = new List<EmployeData>();
            for (int i = 0; i < employeManagerSaveData.Employedatas.Length; i++)
            {
                employeDatas.Add(employeManagerSaveData.Employedatas[i]);
            }

            for (int i = employeDatas.Count - 1; i > -1; i--)
            {
                m_CanHireEmployes[i].AssignEmployeData(employeDatas[i]);
                m_CanHireEmployes[i].HireEmploye();
                employeDatas.Remove(employeDatas[i]);
            }
        }
        StartCoroutine(RefreshEmployesForHire());
    }

    public void StartSave()
    {
        EmployeManagerTask employeManagerTask = new EmployeManagerTask(SaveEmployes);
        IAsyncResult asyncResult = employeManagerTask.BeginInvoke(null, null);
    }

    private void SaveEmployes()
    {
        EmployeManagerSaveData employeManagerSaveData = new EmployeManagerSaveData();
        employeManagerSaveData.Employedatas = new EmployeData[m_HiredEmployes.Count];
        for (int i = 0; i < m_HiredEmployes.Count; i++)
        {
            employeManagerSaveData.Employedatas[i] = m_HiredEmployes[i].GetEmployeData();
        }
        employeManagerSaveData.IDcount += m_Id;
        m_FileOperator.WriteFile(employeManagerSaveData);
    }

    public void AddHireEmploye(Employe employe)
    {
        employe.transform.SetParent(m_InHireTab);
        m_CanHireEmployes.Remove(employe);
        m_HiredEmployes.Add(employe);
    }

    public void FireEmploye(Employe employe)
    {
        m_HiredEmployes.Remove(employe);
        Destroy(employe.gameObject);
    }

    private void SetNames()
    {
        m_Names = new string[]
        {
            "Lukas Smith",
            "Steven Hoven",
            "Ryan Blazer",
            "Norbet van Dijk",
            "Isa-bella Jonkman",
            "Ryan Swart",
            "Allan Vonk",
            "Senna Krebbers",
            "Hidde Neutenboom",
            "Yelena de Wit",
            "Frank Dev",
            "Rubin Pine",
            "Selena Bloom",
            "Jordi Blue",
            "Mats de Wind",
            "Mathijs van Dijk",
            "Pieter van Dijk",
            "Chris van Duin",
            "Sevilay de Mol",
            "Stan de Groot",
        };
    }

    private IEnumerator RefreshEmployesForHire()
    {
        while (true)
        {
            DestroyAllNotHired();

            for (int i = 0; i < m_AtleastAmountOfEmp; i++)
            {
                GameObject gameObject = Instantiate(m_EmployePrefab, m_Tab);
                int nameindex = UnityEngine.Random.Range(0, m_Names.Length);
                float rating = UnityEngine.Random.Range(0, 5);
                EmployeData employeData = new EmployeData
                {
                    Name = m_Names[nameindex],
                    Age = UnityEngine.Random.Range(18, 60),
                    Rating = rating,
                    WorkingSpeed = GetWorkingSpeed(rating),
                    Salary = GetSalary(rating),
                    ID = m_Id,
                    YearsOfService = 0,
                    SendedEmail = false,
                    Hired = false,
                    AmountToLevelUp = GetAmountToLevelUp(rating),
                };
                m_Id++;

                Employe employe = gameObject.GetComponent<Employe>();
                employe.AssignEmployeData(employeData);
                m_CanHireEmployes.Add(employe);
            }
            HideOrShowHiredEmployes(false);
            HideOrShowHireEmployes(false);

            yield return new WaitForSeconds(m_EmployeRefreshCooldown);
        }
    }

    private void DestroyAllNotHired()
    {
        for (int i = 0; i < m_CanHireEmployes.Count; i++)
        {
            Destroy(m_CanHireEmployes[i].gameObject);
        }
        m_CanHireEmployes.Clear();
    }

    private void SpawnFirst(int amount)
    {
        DestroyAllNotHired();

        for (int i = 0; i < amount; i++)
        {
            GameObject gameObject = Instantiate(m_EmployePrefab, m_Tab);
            int nameindex = UnityEngine.Random.Range(0, m_Names.Length);
            float rating = UnityEngine.Random.Range(0, 5);
            EmployeData employeData = new EmployeData
            {
                Name = m_Names[nameindex],
                Age = UnityEngine.Random.Range(18, 60),
                Rating = rating,
                WorkingSpeed = GetWorkingSpeed(rating),
                Salary = GetSalary(rating),
                ID = m_Id,
                YearsOfService = 0,
                SendedEmail = false,
                Hired = false,
                AmountToLevelUp = GetAmountToLevelUp(rating),
            };
            m_Id++;

            Employe employe = gameObject.GetComponent<Employe>();
            employe.AssignEmployeData(employeData);
            m_CanHireEmployes.Add(employe);
        }
        HideOrShowHiredEmployes(false);
        HideOrShowHireEmployes(false);
    }

    public void AgeEmployes()
    {
        for (int i = 0; i < m_HiredEmployes.Count; i++)
        {
            EmployeData employeDataq = m_HiredEmployes[i].GetEmployeData();
            employeDataq.Age++;
            employeDataq.YearsOfService++;
            m_HiredEmployes[i].SetEmployeDayta(employeDataq);
        }
        for (int i = 0; i < m_CanHireEmployes.Count; i++)
        {
            EmployeData employeDataq = m_CanHireEmployes[i].GetEmployeData();
            employeDataq.Age++;
            employeDataq.YearsOfService++;
            m_CanHireEmployes[i].SetEmployeDayta(employeDataq);
        }
    }

    public Employe GetEmploye(EmployeData data)
    {
        for (int i = 0; i < m_HiredEmployes.Count; i++)
        {
            if (m_HiredEmployes[i].GetId() == data.ID)
            {
                return m_HiredEmployes[i];
            }
        }
        return null;
    }

    public void HideOrShowHireEmployes(bool boolean)
    {
        for (int i = 0; i < m_CanHireEmployes.Count; i++)
        {
            m_CanHireEmployes[i].HideOrShow(boolean);
        }
    }

    public void HideOrShowHiredEmployes(bool boolean)
    {
        if (m_HiredEmployes != null)
        {
            for (int i = 0; i < m_HiredEmployes.Count; i++)
            {
                m_HiredEmployes[i].HideOrShow(boolean);
            }
        }
    }

    private void OnApplicationQuit()
    {
        StartSave();
    }

    private int GetWorkingSpeed(float Rating)
    {
        int workingspeed = UnityEngine.Random.Range(5, 10);
        workingspeed = Mathf.RoundToInt(workingspeed * (Rating / 10f));
        return workingspeed;
    }

    private int GetSalary(float rating)
    {
        int salary = UnityEngine.Random.Range(50, 80);
        rating = 1f + (rating / 10f);
        salary = Mathf.RoundToInt((float)salary * rating);
        return salary;
    }

    private int GetAmountToLevelUp(float rating)
    {
        if (rating != 5)
        {
            int amount = UnityEngine.Random.Range(100, 120);
            if (rating == 0)
            {
                amount = Mathf.RoundToInt(amount * 1f);
            }
            else
            {
                amount = Mathf.RoundToInt(amount * rating);
            }
            return amount;
        }
        return 0;
    }

    public Employe[] GetHiredEmployes()
    {
        return m_HiredEmployes.ToArray();
    }
}

[Serializable]
public struct EmployeManagerSaveData
{
    public EmployeData[] Employedatas;
    public int IDcount;
}