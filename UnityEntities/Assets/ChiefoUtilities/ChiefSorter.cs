using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Reflection;

namespace ChiefoUtilities
{
    public enum ValueType
    {
        Int,
        Float,
        Double,
    }

    public static class ChiefSorter
    {
        public static T[] SortArrayBasedOfField<T>(T[] _array, ValueType valueType, string _fieldName)
        {
            switch (valueType)
            {
                case ValueType.Int:
                    return SortArrayBasedOfFieldInt<T>(_array, _fieldName);

                case ValueType.Float:
                    return SortArrayBasedOfFieldFloat<T>(_array, _fieldName);

                case ValueType.Double:
                    return SortArrayBasedOfFieldDouble<T>(_array, _fieldName);
            }
            return null;
        }

        private static T[] SortArrayBasedOfFieldInt<T>(T[] _array, string _fieldName)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                for (int j = 0; j < _array.Length; j++)
                {
                    int _value1 = GetFieldValue<int, T>(_array[i], _fieldName);
                    int _value2 = GetFieldValue<int, T>(_array[j], _fieldName);
                    if (_value1 < _value2)
                    {
                        T _temp = _array[i];
                        _array[i] = _array[j];
                        _array[j] = _temp;
                    }
                }
            }
            return _array;
        }

        private static T[] SortArrayBasedOfFieldFloat<T>(T[] _array, string _fieldName)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                for (int j = 0; j < _array.Length; j++)
                {
                    float _value1 = GetFieldValue<float, T>(_array[i], _fieldName);
                    float _value2 = GetFieldValue<float, T>(_array[j], _fieldName);
                    if (_value1 < _value2)
                    {
                        T _temp = _array[i];
                        _array[i] = _array[j];
                        _array[j] = _temp;
                    }
                }
            }
            return _array;
        }

        private static T[] SortArrayBasedOfFieldDouble<T>(T[] _array, string _fieldName)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                for (int j = 0; j < _array.Length; j++)
                {
                    double _value1 = GetFieldValue<double, T>(_array[i], _fieldName);
                    double _value2 = GetFieldValue<double, T>(_array[j], _fieldName);
                    if (_value1 < _value2)
                    {
                        T _temp = _array[i];
                        _array[i] = _array[j];
                        _array[j] = _temp;
                    }
                }
            }
            return _array;
        }

        public static Vector3[] SortOnClosest(Vector3[] _points, Vector3 _target)
        {
            Vector3 _temp = Vector3.zero;
            for (int i = 0; i < _points.Length; i++)
            {
                for (int j = 0; j < _points.Length; j++)
                {
                    float _Prdis = Vector3.Distance(_points[i], _target);
                    float _NeDis = Vector3.Distance(_points[j], _target);
                    if (_Prdis < _NeDis)
                    {
                        _temp = _points[j];
                        _points[j] = _points[i];
                        _points[i] = _temp;
                    }
                }
            }
            return _points;
        }

        public static int[] SortArray(int[] _array)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                for (int j = 0; j < _array.Length; j++)
                {
                    if (_array[i] < _array[j])
                    {
                        int _temp = _array[i];
                        _array[i] = _array[j];
                        _array[j] = _temp;
                    }
                }
            }
            return _array;
        }

        public static float[] SortArray(float[] _array)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                for (int j = 0; j < _array.Length; j++)
                {
                    if (_array[i] < _array[j])
                    {
                        float _temp = _array[i];
                        _array[i] = _array[j];
                        _array[j] = _temp;
                    }
                }
            }
            return _array;
        }
        public static double[] SortArray(double[] _array)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                for (int j = 0; j < _array.Length; j++)
                {
                    if (_array[i] < _array[j])
                    {
                        double _temp = _array[i];
                        _array[i] = _array[j];
                        _array[j] = _temp;
                    }
                }
            }
            return _array;
        }


        public static TFieldType GetFieldValue<TFieldType, TObjectType>(this TObjectType obj, string fieldName)
        {
            FieldInfo fieldInfo = obj.GetType().GetField(fieldName,
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic);
            return (TFieldType)fieldInfo.GetValue(obj);
        }
    }
}