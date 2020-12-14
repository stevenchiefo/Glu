using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorOrb : MonoBehaviour
{
    public float m_Amount { get; private set; }
    private Vector3 NeededSize = new Vector3(1,1);

    private void Update()
    {
        if(transform.localScale != NeededSize)
        {
            transform.localScale = Vector3.LerpUnclamped(transform.localScale, NeededSize, Time.deltaTime);
        }
    }
    public void SetSize(float size)
    {
        if (size < 6)
        {
            m_Amount = size;
            NeededSize = new Vector3(size, size);
        }
    }

    

    public int FarmOrb(int ammount)
    {
        float futureAmmount = m_Amount - ammount;
        float LeftOver = ammount - m_Amount;
        if(futureAmmount < 0)
        {
            m_Amount = 0;
            SetSize(0);
            return Mathf.RoundToInt(LeftOver);
        }
        SetSize(futureAmmount);
        return Mathf.RoundToInt(ammount);
    }
}