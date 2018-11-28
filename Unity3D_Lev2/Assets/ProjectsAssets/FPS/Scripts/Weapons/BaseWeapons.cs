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
        //но не совсем правильно, т.к. стрелы д.б. в калчане(обойме)
        [SerializeField]
        public int CartridgeHolder;
        [SerializeField]
        public int MaxCartridgeHolder;

        protected float _lastShootTime;

        //public - видимо не правильно, но куда его запихнуть - не понятно
        private float _timeFirePressedDownBtn;

        public void FireBtnPressed(float time)
        {
            _timeFirePressedDownBtn = time;
        }

        public bool TryShoot()
        {
            if (Time.time - _lastShootTime < _timeout) return false;
            _lastShootTime = Time.time;

            //усилили силу, но для огнестрела не пойдёт.
            var oldForce = _force;
            _force *= Mathf.Min(2, Time.time - _timeFirePressedDownBtn) *10;

            Fire();

            _force = oldForce;

            return true;
        }

        protected abstract void Fire();
        public abstract void Reload();
    }
}