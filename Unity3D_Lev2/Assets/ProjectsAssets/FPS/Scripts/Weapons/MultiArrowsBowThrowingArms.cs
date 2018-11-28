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
        private int _etalonCartridge;

        private MultiArrowsBowThrowingArms()
        {
            _etalonCartridge = CartridgeHolder;
        }

        protected override void Fire()
        {
            //var arrow = Instantiate(_WeaponChargeItemPrefab);
            //arrow.Initialize(_firepoint[_currentFirepoint % _firepoint.Length], _force);

            //_currentFirepoint++;

            foreach(var arrowFP in _firepoint)
            {
                var arrow = Instantiate(_WeaponChargeItemPrefab);
                if (CartridgeHolder > 0)
                {
                    arrow.Initialize(arrowFP, _force);
                    CartridgeHolder -= 1;
                }
            }
        }

        public override void Reload()
        {
            CartridgeHolder = _etalonCartridge;
        }
    }
}