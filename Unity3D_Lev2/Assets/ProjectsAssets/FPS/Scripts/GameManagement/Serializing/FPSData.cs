using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class FPSData
    {
        public string PlayerName;
        public float Health;
        public Vector3 PlayerPosition;

        public override string ToString()
        {
            return $"PlayerName: {PlayerName}, Health: {Health}, Coordinates: {PlayerPosition}";
        }
    }

}