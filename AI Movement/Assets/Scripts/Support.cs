using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support
{
    public static void DrawRay(Vector3 pos, Vector3 direction, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);
    }

    public static void DrawLabel(Vector3 pos, string label, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawIcon(pos, label, true);
    }

    public static void Point(Vector3 pos, float Radius, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pos, Radius);
    }

    public static void DrawLine(Vector3 pos1, Vector3 pos2, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(pos1, pos2);
    }
}
