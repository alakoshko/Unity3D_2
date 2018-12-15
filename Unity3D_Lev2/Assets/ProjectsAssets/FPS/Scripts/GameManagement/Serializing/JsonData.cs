using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FPS
{
    public class JsonData : IDataProvider
    {
        string _path;

        public FPSData Load()
        {
            if (!File.Exists(_path)) return default(FPSData);

            var str = File.ReadAllText(_path);
            var fpsData = JsonUtility.FromJson<FPSData>(str);

            Debug.Log("Json data loaded");

            return fpsData;
        }

        public void Save(FPSData fPSData)
        {
            var str = JsonUtility.ToJson(fPSData);
            File.WriteAllText(_path, str);
            Debug.Log("Json data saved");
        }

        public void SetOption(string path)
        {
            _path = Path.Combine(path, "JsonData.json");
        }
    }
}