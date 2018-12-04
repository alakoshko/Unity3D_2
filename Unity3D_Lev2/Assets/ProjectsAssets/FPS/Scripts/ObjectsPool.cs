using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    class ObjectsPool : MonoBehaviour
    {
        public static ObjectsPool Instance { get; private set; }

        [SerializeField]
        private GameObject[] _objects;
        private Dictionary<string, Queue<IPoolable>> _objectDict = new Dictionary<string, Queue<IPoolable>>();


        private void Awake()
        {
            if (Instance) DestroyImmediate(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            foreach(var obj in _objects)
            {
                var poolObj = obj.GetComponent<IPoolable>();
                if (poolObj == null) continue;

                var queue = new Queue<IPoolable>();
                for (int i = 0; i < poolObj.ObjectsCount; i++)
                {
                    GameObject go = Instantiate(obj);
                    go.SetActive(false);
                    queue.Enqueue(go.GetComponent<IPoolable>());
                }
                _objectDict.Add(poolObj.PoolID, queue);
            }
        }

        public IPoolable GetObject(string pullId)
        {
            if (string.IsNullOrEmpty(pullId)) return null;
            if (!_objectDict.ContainsKey(pullId)) return null;

            IPoolable p = _objectDict[pullId].Dequeue();
            _objectDict[pullId].Enqueue(p);
            return p;
        }
    }
}
