using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class SingleArrowBowThrowingArms : BaseWeapons
    {
        [SerializeField]
        private Transform _firepoint;

        protected override void Fire()
        {
            var arrow = Instantiate(_WeaponChargeItemPrefab);
            arrow.Initialize(_firepoint, _force);
        }

        public override void Reload()
        {
            throw new System.NotImplementedException();
        }
    }
}