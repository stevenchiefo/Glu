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

    public static void DrawWiredSphere(Vector3 pos,float radius, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(pos, radius);
    }

    public static void DrawCircle(Vector3 pos, float radius, Color color)
    {
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawWireDisc(pos, new Vector3(0,90,0), radius);
    }

    public static T[] CheckForNearbyObjects<T>(Vector3 _vector3,float _range, LayerMask _layerMask)
    {
        Collider[] _Colls = Physics.OverlapSphere(_vector3, _range, _layerMask);
        T[] _Tarray = new T[_Colls.Length];
        for (int i = 0; i < _Tarray.Length; i++)
        {
            T _t = _Colls[i].GetComponent<T>();
            if(_t != null)
            {
                _Tarray[i] = _t;
            }
        }
        return _Tarray;
    }

    public static T[] CheckForNearbyObjects<T>(Vector3 _vector3, float _range)
    {
        Collider[] _Colls = Physics.OverlapSphere(_vector3, _range);
        T[] _Tarray = new T[_Colls.Length];
        for (int i = 0; i < _Tarray.Length; i++)
        {
            T _t = _Colls[i].GetComponent<T>();
            if (_t != null)
            {
                _Tarray[i] = _t;
            }
        }
        return _Tarray;
    }
}
