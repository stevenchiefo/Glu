using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UnityFileManager 
{
    public string FilePath { get; private set; }

    public UnityFileManager(string _filePath)
    {
        FilePath = _filePath;
    }

    public void WriteFile(object _obj)
    {
        string jsonString = JsonUtility.ToJson(_obj);
        using(StreamWriter writer = File.CreateText(FilePath))
        {
            writer.Write(jsonString);
        }
    }

    public T readFile<T>()
    {
        using (StreamReader reader = File.OpenText(FilePath))
        {
            T _obj = JsonUtility.FromJson<T>(reader.ReadToEnd());
            return _obj;
        }
    }
}
