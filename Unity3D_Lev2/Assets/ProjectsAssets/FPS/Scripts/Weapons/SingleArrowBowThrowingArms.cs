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
            //var arrow = Instantiate(_WeaponChargeItemPrefab);
            
                BaseAmmo arrow = ObjectsPool.Instance.GetObject(_WeaponChargeItemId) as BaseAmmo;
                arrow.Initialize(_firepoint, _force);
                //GetComponent<Animation>().Play("Fire1");
                CartridgeHolder -= 1;
            

        }

        public override void Reload()
        {
            CartridgeHolder = MaxCartridgeHolder;
        }
    }
}