using System.IO;
using UnityEngine;
using UnityEditor;

namespace Maze.Editor
{
    public class Save2Json : IData
    {
        private string _path;

        [MenuItem("Maze/Сохранить лабиринт", false, 0)]
        public static void MazeSave()
        {
            var sceneObj = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];   // Находим все объекты на сцене
            if (sceneObj != null)
            {
                foreach (var obj in sceneObj)
                {
                    var saveJson = new Save2Json();
                    saveJson.SetOptions(Application.temporaryCachePath);
                    saveJson.Save(obj);
                    Debug.Log($"{obj}");
                }
            }
        }

        public void Save(GameObject obj)
        {
            var str = JsonUtility.ToJson(obj);
            File.WriteAllText(_path, str);
        }

        //[MenuItem("Maze/Load")]
        public GameObject Load()
        {
            var str = File.ReadAllText(_path);
            return JsonUtility.FromJson<GameObject>(str);
        }

        public void SetOptions(string path)
        {
            _path = Path.Combine(path, "Data.Maze");
        }
    }
}
