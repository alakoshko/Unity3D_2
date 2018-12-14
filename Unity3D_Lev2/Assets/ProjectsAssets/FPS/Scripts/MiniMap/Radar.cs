using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FPS
{
    public class Radar : MonoBehaviour
    {
        private Transform _playerPos;     // Позиция главного героя
        private readonly float mapScale = 1;
        public static List<RadarObject> RadObjects = new List<RadarObject>();

        private void Start()
        {
            //_playerPos = GameObject.FindGameObjectWithTag("FPSController").transform;
            _playerPos = PlayerModel.LocalPlayer.transform;
        }

        public static void RegisterRadarObject(GameObject o, Image i)
        {
            Image image = Instantiate(i);
            RadObjects.Add(new RadarObject { Owner = o, Icon = image });
        }

        public static void RemoveRadarObject(GameObject o)
        {
            List<RadarObject> newList = new List<RadarObject>();
            foreach (RadarObject t in RadObjects)
            {
                if (t.Owner == o)
                {
                    Destroy(t.Icon);
                    continue;
                }
                newList.Add(t);
            }
            RadObjects.RemoveRange(0, RadObjects.Count);
            RadObjects.AddRange(newList);
        }

        private void DrawRadarDots() // Синхронизирует значки на миникарте с реальными объектами
        {
            foreach (RadarObject radObject in RadObjects)
            {
                //обнуляем y, чтобы сократить дистанцию на карте между летающими и ходящими
                Vector3 vOwner = radObject.Owner.transform.position;
                vOwner.y = 0;
                Vector3 vPlayer = _playerPos.position;
                vPlayer.y = 0;

                Vector3 radarPos2 = (vOwner - vPlayer);
                float distToObject2 = Vector3.Distance(vPlayer, vOwner) * mapScale;

                //Vector3 radarPos = (radObject.Owner.transform.position - _playerPos.position);
                //float distToObject = Vector3.Distance(_playerPos.position, radObject.Owner.transform.position) * mapScale;
                //float deltay = Mathf.Atan2(radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - _playerPos.eulerAngles.y;
                //radarPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
                //radarPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);
                float deltay = Mathf.Atan2(radarPos2.x, radarPos2.z) * Mathf.Rad2Deg - 270 - _playerPos.eulerAngles.y;
                radarPos2.x = distToObject2 * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
                radarPos2.z = distToObject2 * Mathf.Sin(deltay * Mathf.Deg2Rad);

                radObject.Icon.transform.SetParent(transform);
                //radObject.Icon.transform.position = new Vector3(radarPos.x, radarPos.z, 0) + transform.position;
                radObject.Icon.transform.position = new Vector3(radarPos2.x, radarPos2.z, 0) + transform.position;
            }
        }

        private void Update()
        {
            if (Time.frameCount % 3 == 0)
            {
                DrawRadarDots();
            }
        }
    }

    public class RadarObject
    {
        public Image Icon;
        public GameObject Owner;
    }

}