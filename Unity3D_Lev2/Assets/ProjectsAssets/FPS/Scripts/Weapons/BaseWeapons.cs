using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class BaseWeapons : BaseSceneObject
    {
        [SerializeField]
        protected string _WeaponChargeItemId;
        [SerializeField]
        protected float _force;
        [SerializeField]
        protected float _reloadTime;
        [SerializeField]
        protected float _timeout;
        //но не совсем правильно, т.к. стрелы д.б. в калчане(обойме)
        [SerializeField]
        public int CartridgeHolder;
        [SerializeField]
        public int MaxCartridgeHolder;

        private enum weaponsType
        {
            Throwing,
            Firearms
        };
        [SerializeField]
        private weaponsType _weaponsType;

        protected float _lastShootTime;

        //public - видимо не правильно, но куда его запихнуть - не понятно
        private float _fireForce;

        public float FireForce
        {
            get { return _fireForce; }
            set { _fireForce = value; }
        }

        public bool TryShoot()
        {
            if (CartridgeHolder <= 0) return false;
            if (Time.time - _lastShootTime < _timeout) return false;

            _lastShootTime = Time.time;

            if (_weaponsType == weaponsType.Throwing)
            {
                //усилили силу, но для огнестрела не пойдёт.
                var oldForce = _force;
                _force *= Mathf.Min(10, Mathf.Abs(_fireForce) * 10);
                Debug.Log($"force: {_force}");

                Fire();

                _force = oldForce;
            }
            else
                Fire();

            return true;
        }

        protected abstract void Fire();
        public abstract void Reload();
    }
}