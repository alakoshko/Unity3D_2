using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FPS
{
    public class StreamData : IDataProvider
    {
        string _path;
        public FPSData Load()
        {
            if (!File.Exists(_path)) return default(FPSData);

            var fpsData = new FPSData();
            using (var sr = new StreamReader(_path))
            {
                //get PlayerName
                fpsData.PlayerName = sr.ReadLine();

                //get Health
                if (!float.TryParse(sr.ReadLine(), out fpsData.Health))
                {
                    Exception exception = new Exception($"Cannot load Health to float from {_path}");
                }

                //get Position
                var ts = sr.ReadLine();
                fpsData.PlayerPosition = ConvertFunctions.GetVector3(ts);
                
            }
            Debug.Log("Data loaded");
            return fpsData;
        }

        private IDisposable StreamReader(string path)
        {
            throw new NotImplementedException();
        }

        public void Save(FPSData fPSData)
        {
            using(var sw = new StreamWriter(_path))
            {
                sw.WriteLine($"{fPSData.PlayerName}");
                sw.WriteLine($"{fPSData.Health}");
                sw.WriteLine($"{fPSData.PlayerPosition}");
            }
            Debug.Log("Data saved");
        }

        public void SetOption(string path)
        {
            _path = Path.Combine(path, "streamData.txt");
        }
    }

}