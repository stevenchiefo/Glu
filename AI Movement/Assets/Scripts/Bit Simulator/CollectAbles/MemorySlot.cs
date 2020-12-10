using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySlot : MonoBehaviour
{
    private Collider2D m_Collider2D;

    private void Start()
    {
    }

    public Vector2 GetClosestPoint(Vector2 vector2)
    {
        return m_Collider2D.ClosestPoint(vector2);
    }
}