using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class CenterLook : MonoBehaviour
    {
        public float distanceValue = 10f;
        // Use this for initialization
        private void Update()
        {
            transform.LookAt(Camera.main.ViewportToWorldPoint(new Vector3(.71f, .35f, distanceValue)));
        }
    }
}