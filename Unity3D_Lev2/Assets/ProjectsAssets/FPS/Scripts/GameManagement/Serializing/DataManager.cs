using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class DataManager 
    {
        private IDataProvider _dataProvider;

        public void SetData<T>() where T : IDataProvider, new() => _dataProvider = new T();

        public void Save(FPSData fpsData) => _dataProvider?.Save(fpsData);

        public FPSData Load() => _dataProvider == null ? default(FPSData) : _dataProvider.Load();

        public void SetOption(string path) => _dataProvider?.SetOption(path);
    }
}