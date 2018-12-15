using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public interface IDataProvider
    {
        void Save(FPSData fPSData);
        FPSData Load();
        void SetOption(string path);
    }
}