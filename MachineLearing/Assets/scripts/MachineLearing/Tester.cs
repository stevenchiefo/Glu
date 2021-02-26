using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
    [Header("UI FeedBack")]
    [SerializeField] private Slider m_Slider;

    [SerializeField] private Toggle m_BestMachineToggle;

    [SerializeField] private Text m_SliderAmount;
    [SerializeField] private Text m_TestCounter;
    [SerializeField] private Text m_FeedBackText;

    [SerializeField] private Transform m_StartPostionForTest;
    [SerializeField] private Transform m_FinishPosition;
    [SerializeField] private GameObject m_Prefab;
    private MachineData m_Data;

    private void Start()
    {
        m_Slider.onValueChanged.AddListener(UpdateSliderAmount);
        UpdateSliderAmount(m_Slider.value);
    }

    public void UpdateSliderAmount(float amount)
    {
        m_SliderAmount.text = amount.ToString();
    }

    public void OnStartTest()
    {
        StartCoroutine(Test((int)m_Slider.value));
    }

    public IEnumerator Test(int amountTests)
    {
        Time.timeScale = 25f;
        MachineData[] _Datas = new MachineData[amountTests];
        for (int i = 0; i < amountTests; i++)
        {
            m_TestCounter.text = "Current Test: " + (i + 1).ToString();

            GameObject obj = Instantiate(m_Prefab, m_StartPostionForTest.position, Quaternion.identity);
            Machine machine = obj.GetComponent<Machine>();
            if (m_BestMachineToggle.isOn && m_Data.m_Brain.FrontPerceptron != null)
            {
                machine.SetBrain(m_Data.m_Brain);
                machine.Brain.Train(2f);
            }
            yield return new WaitUntil(() => machine.Failed || machine.Finished);
            float _dis = -(Vector3.Distance(m_FinishPosition.position, machine.transform.position) - 100);
            int addamount = 0;
            if (machine.Finished)
                addamount = 20;
            _Datas[i] = new MachineData
            {
                Score = _dis + addamount,
                m_Brain = machine.Brain,
            };
            Destroy(machine.gameObject);
        }
        MachineData _newbestData = GetBestData(_Datas);
        if (m_Data.Score < _newbestData.Score)
            m_Data = _newbestData;
        Time.timeScale = 1f;
        SetFeedBack(_Datas);
    }

    private void SetFeedBack(MachineData[] _datas)
    {
        TestFeedBack testFeedBack = new TestFeedBack(_datas);
        m_FeedBackText.text = testFeedBack.GetFeedBack();
    }

    private MachineData GetBestData(MachineData[] _data)
    {
        if (_data == null)
            return default;
        MachineData machineData = _data[0];
        for (int i = 0; i < _data.Length; i++)
        {
            if (machineData.Score < _data[i].Score)
            {
                machineData = _data[i];
            }
        }
        return machineData;
    }
}

public class TestFeedBack
{
    private MachineData[] m_Datas;
    private string m_FeedBack;

    public TestFeedBack(MachineData[] _Datas)
    {
        m_Datas = _Datas;
        CacaluteFeedBack();
    }

    private void CacaluteFeedBack()
    {
        float sum = 0f;
        foreach (MachineData i in m_Datas)
        {
            sum += i.Score;
        }
        sum /= m_Datas.Length;
        m_FeedBack += $"The average Score was: [{sum}]\n";
    }

    public string GetFeedBack()
    {
        return m_FeedBack;
    }
}

public struct MachineData
{
    public Brain m_Brain;
    public float Score;
}