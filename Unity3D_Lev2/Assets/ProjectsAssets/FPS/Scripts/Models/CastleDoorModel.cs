using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class CastleDoorModel : MonoBehaviour
    {
        public float StateAmount { get; private set; }

        [SerializeField]
        private bool _isOpened;
        public bool isOpened {
            get { return _isOpened; }
            set { _isOpened = value; }
        }
        [SerializeField]
        private float _closeMax;
        [SerializeField]
        private float _closeMin;

        private Transform _currentPosition;
        
        public IEnumerable Open(bool b)
        {
            float actionLimit;
            if (b)
                actionLimit = _closeMax;
            else
                actionLimit = _closeMin;

            float _currentPositionY;
            do
            {
                yield return new WaitForSeconds(1f);
                if(b)
                    transform.position += Vector3.up;
                else
                    transform.position -= Vector3.up;

                _currentPositionY = gameObject.GetComponent<Transform>().position.y;

            } while (_currentPositionY >= actionLimit);
        }
    }
}