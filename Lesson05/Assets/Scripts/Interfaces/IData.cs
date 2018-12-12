using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Editor
{
    public interface IData
    {
        void Save(GameObject maze);
        GameObject Load();
        void SetOptions(string path);
    }
}