using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusUI : MonoBehaviour
{
    [SerializeField] private Image m_MbBar;
    [SerializeField] private Image m_ProcessBar;
    [SerializeField] private Image m_MatingBar;

    private ViriusBrain m_Virus;

    public void SetBrain(ViriusBrain dataCubeBrain)
    {
        m_Virus = dataCubeBrain;
        UpdateUi();
    }

    public void UpdateUi()
    {
        float mbFill = (float)m_Virus.m_CurrentMB / (float)EnitiyManager.instance.ViriusSettings.m_MaxMb;
        m_MbBar.fillAmount = mbFill;
        float CpuProcess = (float)m_Virus.m_CurrentProcess / (float)EnitiyManager.instance.ViriusSettings.m_MaxProcess;
        m_ProcessBar.fillAmount = CpuProcess;
        float ammount = (float)m_Virus.m_LifeTime / (float)EnitiyManager.instance.ViriusSettings.LifeTimeNeeded;
        m_MatingBar.fillAmount = ammount;
    }
}