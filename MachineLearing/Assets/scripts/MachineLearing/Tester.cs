using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Tester : MonoBehaviour
{
    [Header("UI FeedBack")]
    [SerializeField] private Slider m_TrainingAmountSlider;
    [SerializeField] private Slider m_SimulationSpeedSlider;
    [SerializeField] private Slider m_TrainingRateSlider;

    [SerializeField] private Toggle m_BestMachineToggle;

    [SerializeField] private InputField m_SliderAmount;
    [SerializeField] private Text m_TestCounter;
    [SerializeField] private Text m_TrainRateText;
    [SerializeField] private Text m_SimulationSpeedText;
    [SerializeField] private Text m_FeedBackText;

    [Header("Settings")]
    [SerializeField] private Transform m_StartPostionForTest;
    [SerializeField] private Transform m_FinishPosition;
    [SerializeField] private GameObject m_Prefab;
    private MachineData m_Data;

    private UnityFileManager m_FileManager;
    private bool m_killCurrentTest;
    private bool m_KillSimulation;

    public void OnStartTest()
    {
        StartCoroutine(Test((int)m_TrainingAmountSlider.value));
    }

    public void OnKillCurrentTest()
    {
        m_killCurrentTest = true;
    }

    public void OnKillCurrentSimulation()
    {
        m_KillSimulation = true;
    }
    private void Start()
    {
        m_TrainingAmountSlider.onValueChanged.AddListener(UpdateSliderAmount);
        m_FileManager = new UnityFileManager("MachineBrain.json");
        m_SliderAmount.onValueChanged.AddListener(CheckLegalString);
        m_SimulationSpeedSlider.onValueChanged.AddListener(CheckSimulationSpeed);
        m_TrainingRateSlider.onValueChanged.AddListener(OnTrainRateChanged);
        UpdateSliderAmount(m_TrainingAmountSlider.value);
    }

    private void CheckLegalString(string amount)
    {
        int value = (int)m_TrainingAmountSlider.value;
        try
        {
            value = Convert.ToInt32(amount);
            if (value > m_TrainingAmountSlider.maxValue)
            {
                value = (int)m_TrainingAmountSlider.maxValue;
            }
        }
        catch (Exception e)
        {
            m_FeedBackText.text += $"Error: [{ e}]\n";
        }
        m_TrainingAmountSlider.value = value;
        m_SliderAmount.text = value.ToString();
    }

    private void CheckSimulationSpeed(float amount)
    {
        Time.timeScale = amount;
        m_SimulationSpeedText.text = amount.ToString();
    }

    private void OnTrainRateChanged(float amount)
    {
        m_TrainRateText.text = amount.ToString();
    }

    private void UpdateSliderAmount(float amount)
    {
        m_SliderAmount.text = amount.ToString();
    }


    public IEnumerator Test(int amountTests)
    {
        MachineData[] _Datas = new MachineData[amountTests];
        for (int i = 0; i < amountTests; i++)
        {
            m_TestCounter.text = "Current Test: " + (i + 1).ToString();

            GameObject obj = Instantiate(m_Prefab, m_StartPostionForTest.position, Quaternion.identity);
            Machine machine = obj.GetComponent<Machine>();
            if (m_BestMachineToggle.isOn && m_Data.m_Brain.FrontPerceptron != null)
            {
                machine.SetBrain(m_Data.m_Brain);
                machine.Brain.Train(m_TrainingRateSlider.value);
            }
            yield return new WaitUntil(() => machine.Failed || machine.Finished || m_killCurrentTest || m_KillSimulation);
            if (m_KillSimulation)
            {
                m_FeedBackText.text = "Simulation Canceld";
                Destroy(machine.gameObject);
                break;
            }
            else if (m_killCurrentTest)
            {
                m_FeedBackText.text += $"Killed Simulation: [{i}]";
                _Datas[i] = new MachineData
                {
                    Score = 0f,
                    m_Brain = default,
                    Finished = false,
                };
                Destroy(machine.gameObject);
                m_killCurrentTest = false;
                continue;
            }

            float _dis = -(Vector3.Distance(m_FinishPosition.position, machine.transform.position) - 100);
            int addamount = -20;
            if (machine.Finished)
                addamount = 20;
            _Datas[i] = new MachineData
            {
                Score = _dis + addamount,
                m_Brain = machine.Brain,
                Finished = machine.Finished,
            };
            Destroy(machine.gameObject);
        }
        if (m_KillSimulation || m_killCurrentTest)
        {
            m_killCurrentTest = false;
            m_KillSimulation = false;
        }
        else
        {

            MachineData _newbestData = GetBestData(_Datas);
            if (m_Data.Score < _newbestData.Score)
            {
                m_FeedBackText.text += $"Best Bot has been replaced, Previous score: {m_Data.Score}, new score: {_newbestData.Score}\n";
                m_Data = _newbestData;

            }
            SetFeedBack(_Datas);
        }
    }



    private void OnApplicationQuit()
    {
        SaveBrain();
    }

    private void SaveBrain()
    {
        SavePreceptron[] _weights = new SavePreceptron[]
        {
            new SavePreceptron
            {
                Weights = m_Data.m_Brain.FrontPerceptron.Weights,
            },
            new SavePreceptron
            {
                Weights = m_Data.m_Brain.LeftPerceptron.Weights,
            },
            new SavePreceptron
            {
                Weights = m_Data.m_Brain.RightPerceptron.Weights,
            },


        };


        MachineSaveData _data = new MachineSaveData
        {
            Score = m_Data.Score,
            Perceptrons = _weights,
        };
        m_FileManager.WriteFile(_data);
    }

    public void LoadSavedBrain()
    {
        MachineSaveData _data = m_FileManager.readFile<MachineSaveData>();
        Brain brain = new Brain
        {
            FrontPerceptron = new Perceptron(1),
            LeftPerceptron = new Perceptron(1),
            RightPerceptron = new Perceptron(1),

        };
        brain.FrontPerceptron.Weights = _data.Perceptrons[0].Weights;
        brain.LeftPerceptron.Weights = _data.Perceptrons[1].Weights;
        brain.RightPerceptron.Weights = _data.Perceptrons[2].Weights;
        m_Data = new MachineData
        {
            m_Brain = brain,
            Score = _data.Score,
        };
        m_FeedBackText.text += $"Brain succefuly loaded\n";
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

        string _DidFinish = "None Bots has finished the course\n";
        int amountFinished = 0;
        foreach (MachineData i in m_Datas)
        {
            if (i.Finished)
            {
                amountFinished++;
            }
        }

        if (amountFinished > 0)
        {
            _DidFinish = $"[{amountFinished}] Finished The Course\n";
        }
        m_FeedBack += _DidFinish;
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
    public bool Finished;
}

[Serializable]
public struct MachineSaveData
{
    public SavePreceptron[] Perceptrons;
    public float Score;
}
[Serializable]
public struct SavePreceptron
{
    public float[] Weights;
}