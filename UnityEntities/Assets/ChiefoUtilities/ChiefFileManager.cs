using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

namespace ChiefoUtilities
{
    namespace FileManagement
    {
        public class ChiefFileManager
        {
            public static void SaveFile(ref SaveFileInfo _info)
            {
                if(_info.SaveObject != null)
                {
                    string _jsonString = JsonUtility.ToJson(_info.SaveObject);
                    if (_jsonString == "{}")
                        return;
                    _info.SuccesfullSave = true;

                    using(StreamWriter _stream = File.CreateText(_info.FilePath))
                    {
                        _stream.Flush();
                        _stream.Write(_jsonString);
                        _stream.Close();
                    }
                }
            }

            public static T ReadFile<T>(string _filePath)
            {
                if (!File.Exists(_filePath))
                    return default;

                using(StreamReader _reader = new StreamReader(_filePath))
                {
                    string _ReadText = _reader.ReadToEnd();
                    T _object = JsonUtility.FromJson<T>(_ReadText);
                    return _object;
                }
            }
        }

        public struct SaveFileInfo
        {
            public object SaveObject;
            public string FilePath;
            public bool SuccesfullSave;
            public SaveFileInfo(object _saveObject, string _filePath)
            {
                SaveObject = _saveObject;
                FilePath = _filePath;
                SuccesfullSave = false;
            }
        }
    }
}
