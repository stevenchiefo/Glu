using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_DataCubeText;
    [SerializeField] private TextMeshProUGUI m_ViriusText;

    public void UpdateUi()
    {
        string Context = $"DataCubes:\n" +
            $"Alive:{EnitiyManager.instance.AliveDataCubes}\n" +
            $"Born:{EnitiyManager.instance.DataCubesBorn}\n" +
            $"Dead:{EnitiyManager.instance.DeadDataCubes}";
        m_DataCubeText.text = Context;

        Context = $"Virus:\n" +
            $"Alive:{EnitiyManager.instance.AliveVirus}\n" +
            $"Born:{EnitiyManager.instance.VirusBorn}\n" +
            $"Dead:{EnitiyManager.instance.DeadVirus}";
        m_ViriusText.text = Context;
    }
}