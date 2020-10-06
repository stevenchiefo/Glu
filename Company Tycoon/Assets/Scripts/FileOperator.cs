using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileOperator
{
    public string FilePath { get; private set; } //FilePath = Dit is de naam van de path naar je file;

    public FileOperator(string filepath)
    {
        FilePath = filepath;
    }

    public void WriteFile(object _obj)
    {
        using (StreamWriter file = File.CreateText(FilePath))
        {
            string _jsonstring = Tojson(_obj);
            file.Write(_jsonstring);
            file.Close();
        }
    }

    private string Tojson(object _obj)
    {
        return JsonUtility.ToJson(_obj);
    }

    public T ReadFile<T>()
    {
        using (StreamReader file = File.OpenText(FilePath))
        {
            string readed = file.ReadToEnd();
            T obj = JsonUtility.FromJson<T>(readed);
            return obj;
        }
    }

    public bool AlreadyExits()
    {
        return File.Exists(FilePath);
    }
}