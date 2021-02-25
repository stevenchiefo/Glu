using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;
    [SerializeField] private Text m_SliderAmount;
    [SerializeField] private Transform m_StartPostionForTest;
    [SerializeField] private GameObject m_Prefab;
    void Start()
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
        Time.timeScale = 100f;
        for (int i = 0; i < amountTests; i++)
        {
            GameObject obj = Instantiate(m_Prefab, m_StartPostionForTest.position, Quaternion.identity);
            Machine machine = obj.GetComponent<Machine>();
            yield return new WaitUntil(() => machine.Failed);
            Destroy(machine.gameObject);
        }
        Time.timeScale = 1f;

    }

    private MachineData GetBestData(MachineData[] _data)
    {
        MachineData machineData = _data[0];
        for (int i = 0; i < _data.Length; i++)
        {
            if(machineData.distance < _data[i].distance)
            {
                machineData = _data[i];
            }
        }
        return machineData;
    }
}

public struct MachineData
{
    public Brain m_Brain;
    public float distance;
}
