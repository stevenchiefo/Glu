using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySlot : PoolableObject
{
    private Collider2D m_Collider2D;

    private int m_CurrentMB;

    public bool CanFarm;

    public override void Load()
    {
        m_Collider2D = GetComponent<Collider2D>();
        OnPool.AddListener(EnitiyManager.instance.RemoveMemorySlot);
    }

    public void SetMB(int _mb)
    {
        m_CurrentMB = _mb;
        CanFarm = true;
    }

    public int FarmMB(int ammount)
    {
        int futuremb = m_CurrentMB - ammount;
        int leftover = -(ammount - m_CurrentMB);
        if(futuremb < 0)
        {
            CanFarm = false;
            m_CurrentMB = 0;
            PoolObject();
            return leftover;
        }
        m_CurrentMB = futuremb;
        return ammount;
    }

    public Vector2 GetClosestPoint(Vector2 vector2)
    {
        return m_Collider2D.ClosestPoint(vector2);
    }
}