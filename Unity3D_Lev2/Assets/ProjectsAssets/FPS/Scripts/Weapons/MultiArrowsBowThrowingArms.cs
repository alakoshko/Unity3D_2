using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class MultiArrowsBowThrowingArms : BaseWeapons
    {
        [SerializeField]
        private Transform[] _firepoint;
        private int _currentFirepoint;

        protected override void Fire()
        {
            var arrow = Instantiate(_WeaponChargeItemPrefab);
            arrow.Initialize(_firepoint[_currentFirepoint % _firepoint.Length], _force);

            _currentFirepoint++;
        }

        public override void Reload()
        {
            throw new System.NotImplementedException();
        }
    }
}