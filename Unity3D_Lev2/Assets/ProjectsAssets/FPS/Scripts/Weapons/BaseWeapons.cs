using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class BaseWeapons : BaseSceneObject
    {
        [SerializeField]
        protected BaseAmmo _WeaponChargeItemPrefab;
        [SerializeField]
        protected float _force;
        [SerializeField]
        protected float _reloadTime;
        [SerializeField]
        protected float _timeout;

        protected float _lastShootTime;

        //public - видимо не правильно, но куда его запихнуть - не понятно
        public float FirePressDownBtnTime;

        //но не совсем правильно, т.к. стрелы д.б. в калчане(обойме)
        [SerializeField]
        public int CartridgeHolder;

        public bool TryShoot()
        {
            if (Time.time - _lastShootTime < _timeout) return false;
            _lastShootTime = Time.time;

            //усилили силу, но для огнестрела не пойдёт.
            var oldForce = _force;
            _force *= Mathf.Min(5, Time.time - FirePressDownBtnTime)*10;

            Fire();

            _force = oldForce;

            return true;
        }

        protected abstract void Fire();
        public abstract void Reload();
    }
}