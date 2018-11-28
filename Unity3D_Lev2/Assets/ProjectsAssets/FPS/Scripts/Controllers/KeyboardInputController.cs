using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class KeyboardInputController : BaseController
    {
        private void Update()
        {
            if (Input.GetButtonDown("SwitchFlashLight"))
                Main.Instance.FlashLightController.FlashLightSwitch();

            if (Input.GetButtonDown("Fire1"))
                Main.Instance.WeaponsController.SetFireForce();
            if (Input.GetButtonUp("Fire1"))
                Main.Instance.WeaponsController.Fire();

            if (Input.GetButtonDown("ChangeWeapon") )
                Main.Instance.WeaponsController.ChangeWeapon();

            if (Input.GetButtonDown("ReloadCartridge"))
                Main.Instance.WeaponsController.ReloadCartridge();
        }
    }
}