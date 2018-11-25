using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class FlashLightController : BaseController
    {
        private FlashLightModel _model;

        private void Awake()
        {
            _model = FindObjectOfType<FlashLightModel>();
            _model.Off();
        }

        public void FlashLightSwitch()
        {
            if (_model.IsOn) _model.Off();
            else _model.On();
        }
    }
}