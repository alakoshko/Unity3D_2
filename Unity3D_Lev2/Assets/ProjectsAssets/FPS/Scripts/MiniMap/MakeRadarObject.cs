using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class MakeRadarObject : MonoBehaviour
    {
        public Image Image;

        private void Start()
        {
            Radar.RegisterRadarObject(gameObject, Image);
        }

        private void OnDestroy()
        {
            Radar.RemoveRadarObject(gameObject);
        }
    }
}