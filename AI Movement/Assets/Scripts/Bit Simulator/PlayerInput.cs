using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    public float MbMultiPlyer;
    public float CpuMultiPlyer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void OnMBValueChanged(float ammount)
    {
        MbMultiPlyer = ammount;
    }

    public void OnCpuValueChanged(float ammount)
    {
        CpuMultiPlyer = ammount;
    }
}