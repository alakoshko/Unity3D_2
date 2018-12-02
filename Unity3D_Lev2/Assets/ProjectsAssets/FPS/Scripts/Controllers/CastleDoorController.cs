using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class CastleDoorController : BaseController
    {
        private CastleDoorModel _model;

        private void Awake()
        {
            _model = FindObjectOfType<CastleDoorModel>();
            _model.Open(false);
        }

        private void Update()
        {

        }

        public void CastleDoorSwitch()
        {
            if (_model.enabled) _model.Open(true);
            else _model.Open(false);
        }
    }
}