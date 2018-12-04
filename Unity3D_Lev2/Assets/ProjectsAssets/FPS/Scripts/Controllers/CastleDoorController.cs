using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class CastleDoorController : BaseController
    {
        private CastleDoorModel _modelObject;

        private void Awake()
        {
            _modelObject = FindObjectOfType<CastleDoorModel>();
            _modelObject.Open(false);
        }

        private void Update()
        {

        }

        public void CastleDoorSwitch()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                CastleDoorModel castleDoor = hit.collider.GetComponent<CastleDoorModel>();
                if (castleDoor)
                    if (_modelObject.enabled) _modelObject.Open(true);
                    else _modelObject.Open(false);
            }

            
        }
    }
}