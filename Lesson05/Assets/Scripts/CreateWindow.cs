using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace Maze.Editor
{
    public class MazePoint
    {
        public bool Visited = false; // если посещали точку
        public GameObject[] gameObject = new GameObject[4]; //ссылка на объект- стену
    }

    public class MazeWindow : EditorWindow
    {
        public GameObject ObjectInstantiate;
        string _nameObject = "Wall";
        bool groupEnabled;
        bool _randomColor = true;
        int _countObject = 1;
        float _radius = 10;
        float boxSize = 5f;
        int _matrixSizeX;
        int _matrixSizeY;
        int matrixSize;
        MazePoint[,] m1;

        Color[] _colors = new Color[]
        {
            Color.green, Color.black, Color.blue, Color.clear, Color.cyan, Color.red, Color.yellow, Color.white,Color.red
        };

        private static readonly Dictionary<string, int> _nameDictionary = new Dictionary<string, int>();

        [MenuItem("Maze/Wall Creation")]
        public static void WallCreationWindow()
        {
            // Отобразить существующий экземпляр окна. Если его нет, создаем
            EditorWindow.GetWindow(typeof(MazeWindow));
        }


        void OnGUI()
        {
            // Здесь методы отрисовки схожи с методами в пользовательском интерфейсе, который вы разрабатывали на курсе “Unity3D. Уровень 1”
            GUILayout.Label("Базовые настройки", EditorStyles.boldLabel);
            ObjectInstantiate =
                EditorGUILayout.ObjectField("Объект который хотим вставить", ObjectInstantiate, typeof(GameObject), true)
                    as GameObject;
            _nameObject = EditorGUILayout.TextField("Имя объекта", _nameObject);
            groupEnabled = EditorGUILayout.BeginToggleGroup("Дополнительные настройки", groupEnabled);

            _matrixSizeX = EditorGUILayout.IntSlider("Размер матрицы, длина", _matrixSizeX, 3, 10);
            _matrixSizeY = EditorGUILayout.IntSlider("Размер матрицы, ширина", _matrixSizeY, 3, 10);
            matrixSize = _matrixSizeX * _matrixSizeY;

            EditorGUILayout.EndToggleGroup();
            if (GUILayout.Button("Создать объекты"))
            {
                if (ObjectInstantiate)
                {
                    #region from https://habr.com/post/335974/
                    MazeVisualizer mazeVisualizer = new MazeVisualizer();
                    mazeVisualizer.RefreshMaze(_matrixSizeX, _matrixSizeY);
                    #endregion
                }
            }
        }

        

        

        #region MyAlgoritm
        //    private void CreateMaze(GameObject root)
        //    {
        //        float x = 0;
        //        float z = 0;
        //        int nColor = Random.Range(0, _colors.Length - 1);
        //        m1 = new MazePoint[_matrixSideSize, _matrixSideSize];

        //        //Возводим стены
        //        for (int i = 0; i <= _matrixSideSize; i++)
        //        {
        //            for (int j = 0; j <= _matrixSideSize; j++)
        //            {
        //                if (i < _matrixSideSize && j < _matrixSideSize)
        //                {
        //                    m1[i, j] = new MazePoint();
        //                    m1[i, j].Visited = false;
        //                    m1[i, j].gameObject[0] = InstantiateObject(root, $"{_nameObject}-({i}-{j})", x, z, nColor);
        //                    m1[i, j].gameObject[1] = InstantiateObject(root, $"{_nameObject}2-({i}-{j})", x, z, 90, nColor);
        //                }


        //                if (i == _matrixSideSize && j != _matrixSideSize)
        //                    InstantiateObject(root, $"{_nameObject}-({i}-{j})", x, z, nColor);

        //                if (j == _matrixSideSize && i != _matrixSideSize)
        //                    InstantiateObject(root, $"{_nameObject}2-({i}-{j})", x, z, 90, nColor);



        //                z += boxSize;
        //            }

        //            x += boxSize;
        //            z = 0;
        //            nColor = Random.Range(0, _colors.Length - 1);
        //        }

        //        //Бурим ходы
        //        if (_createHoles)
        //            HoleWall(0, 0);

        //        Debug.Log(m1);
        //    }

        //    //вызывать с координатами, например 0,0
        //    private void HoleWall(int i, int j)
        //    {
        //        var rand = new System.Random();

        //        if (m1[i, j].Visited) return;

        //        m1[i, j].Visited = true;

        //        //сгенерировать следующий ход
        //        if (checkMatrixFindEmpty())
        //        {
        //            int r = rand.Next(0, 3);
        //            switch (r)
        //            {
        //                case 0:
        //                    if (i + 1 < _matrixSideSize && !m1[i + 1, j].Visited)
        //                    {
        //                        //удалить стену
        //                        if (m1[i, j].gameObject[r] != null)
        //                            Destroy(m1[i, j].gameObject[r]);
        //                        //перейти туда
        //                        HoleWall(i + 1, j);
        //                    }
        //                    else
        //                    {
        //                        m1[i, j].Visited = false;
        //                        HoleWall(i, j);
        //                    }
        //                    break;
        //                case 1:
        //                    if (j + 1 < _matrixSideSize && !m1[i, j + 1].Visited)
        //                    {
        //                        //удалить стену
        //                        if (m1[i, j].gameObject[r] != null)
        //                            Destroy(m1[i, j].gameObject[r]);
        //                        //перейти туда
        //                        HoleWall(i, j + 1);
        //                    }
        //                    else
        //                    {
        //                        m1[i, j].Visited = false;
        //                        HoleWall(i, j);
        //                    }
        //                    break;
        //                case 2:
        //                    if (j - 1 >= 0 && !m1[i, j - 1].Visited)
        //                    {
        //                        //удалить стену
        //                        if (m1[i, j].gameObject[r] != null)
        //                            Destroy(m1[i, j].gameObject[r]);
        //                        //перейти туда
        //                        HoleWall(i, j - 1);
        //                    }
        //                    else
        //                    {
        //                        m1[i, j].Visited = false;
        //                        HoleWall(i, j);
        //                    }
        //                    break;
        //                case 3:
        //                    if (i - 1 >= 0 && !m1[i - 1, j].Visited)
        //                    {
        //                        //удалить стену
        //                        if (m1[i, j].gameObject[r] != null)
        //                            Destroy(m1[i, j].gameObject[r]);
        //                        //перейти туда
        //                        HoleWall(i - 1, j);
        //                    }
        //                    else
        //                    {
        //                        m1[i, j].Visited = false;
        //                        HoleWall(i, j);
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //    private bool checkMatrixFindEmpty()
        //    {
        //        for (int i = 0; i <= _matrixSideSize; i++)
        //        {
        //            for (int j = 0; j <= _matrixSideSize; j++)
        //            {
        //                if (!m1[i, j].Visited) return true;
        //            }
        //        }
        //        return false;
        //    }

        //    private GameObject InstantiateObject(GameObject root, string name, float x, float z, int nColor)
        //    {
        //        Vector3 pos = new Vector3(x, 0, z);
        //        GameObject temp = Instantiate(ObjectInstantiate, pos, Quaternion.identity) as GameObject;
        //        temp.name = name;
        //        temp.transform.parent = root.transform;
        //        //if (temp.GetComponent<Renderer>() && _randomColor)
        //        //{
        //        //    temp.GetComponent<Renderer>().material.color = _colors[nColor];
        //        //    // Unity предупреждает о возможной утечке памяти и предлагает использовать sharedMaterial
        //        //}
        //        return temp;
        //    }
        //    private GameObject InstantiateObject(GameObject root, string name, float x, float z, int Degree, int nColor)
        //    {
        //        Vector3 pos = new Vector3(x, 0, z);
        //        GameObject temp = Instantiate(ObjectInstantiate, pos, Quaternion.identity) as GameObject;
        //        temp.name = name;
        //        var xOffset = new Vector3(1.5f, 0, 0);
        //        temp.transform.parent = root.transform;
        //        temp.transform.position += xOffset;
        //        temp.transform.Rotate(Vector3.up * Degree, Space.World);
        //        //if (temp.GetComponent<Renderer>() && _randomColor)
        //        //{
        //        //    temp.GetComponent<Renderer>().material.color = _colors[nColor];
        //        //    // Unity предупреждает о возможной утечке памяти и предлагает использовать sharedMaterial
        //        //}
        //        return temp;
        //    }
        #endregion
    }

}
