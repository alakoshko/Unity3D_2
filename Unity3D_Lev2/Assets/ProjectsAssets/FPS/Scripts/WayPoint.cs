using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    public class WayPoint : MonoBehaviour
    {
        public class IntUnityEvent : UnityEvent<int> { }

        public float WaitTime = 1f;
        public IntUnityEvent ReachWPEvents;

        private float _currentWPTimeout;

        public void WPReached()
        {
            ReachWPEvents?.Invoke(1);
        }
    }
}