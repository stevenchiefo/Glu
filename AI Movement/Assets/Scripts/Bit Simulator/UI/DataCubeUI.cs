﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataCubeUI : MonoBehaviour
{
    [SerializeField] private Image m_MbBar;
    [SerializeField] private Image m_ProcessBar;
    [SerializeField] private Image m_MatingBar;

    private DataCubeBrain m_DataCubeBrain;

    public void SetBrain(DataCubeBrain dataCubeBrain)
    {
        m_DataCubeBrain = dataCubeBrain;
        UpdateUi();
    }

    public void UpdateUi()
    {
        float mbFill = (float)m_DataCubeBrain.m_CurrentMB / (float)EnitiyManager.instance.DataCubeSettings.m_MaxMb;
        m_MbBar.fillAmount = mbFill;
        float CpuProcess = (float)m_DataCubeBrain.m_CurrentProcess / (float)EnitiyManager.instance.DataCubeSettings.m_MaxProcess;
        m_ProcessBar.fillAmount = CpuProcess;
        float ammount = (float)m_DataCubeBrain.m_LifeTime / (float)EnitiyManager.instance.DataCubeSettings.LifeTimeNeeded;
        m_MatingBar.fillAmount = ammount;
    }

}