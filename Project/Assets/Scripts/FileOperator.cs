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
            file.Write(_obj);
            file.Close();
        }
    }
}

public class ikmaakfiles
{
    private FileOperator FileOperator;

    public ikmaakfiles()
    {
        FileOperator = new FileOperator("DND.txt");
        FileOperator.WriteFile("Deze dugoen is cleared");
    }
}