using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class InputController : BaseController
    {
        private float mswValue;

        private void Update()
        {
            if (Input.GetButtonDown("SwitchFlashLight"))
                Main.Instance.FlashLightController.FlashLightSwitch();

            if (Input.GetButtonDown("ChangeWeapon") )
                Main.Instance.WeaponsController.ChangeWeapon();

            if (Input.GetButtonDown("ReloadCartridge"))
                Main.Instance.WeaponsController.ReloadCartridge();

            #region do a Fire
            //Set force of fire
            
            if ( Input.GetButton("Fire1"))
            {
                mswValue += Input.GetAxis("Mouse ScrollWheel");
                //if (mswValue != 0f)
                    //Animation of  bow pulling
                    //gameObject.

            }

            //Fire!!!!
            if (Input.GetButtonUp("Fire1"))
            {
                Main.Instance.WeaponsController.SetFireForceFromMSW(mswValue);
                Main.Instance.WeaponsController.Fire();
                mswValue = 0;
            }
            #endregion
        }
    }
}