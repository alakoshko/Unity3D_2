using System;
using UnityEngine;

namespace FPS
{
    public class FlashLightModel : MonoBehaviour
    {
        public event Action<bool> FlashLightStateChanged;

        private Light _light;

        public bool IsOn { get { return _light.enabled; } }

        private void Awake() { _light = GetComponent<Light>(); }

        public void On() {
            _light.enabled = true;
            if (FlashLightStateChanged != null) FlashLightStateChanged.Invoke(true);
        }
        public void Off() {
            _light.enabled = false;
            if (FlashLightStateChanged != null) FlashLightStateChanged.Invoke(false);
        }
    }
}