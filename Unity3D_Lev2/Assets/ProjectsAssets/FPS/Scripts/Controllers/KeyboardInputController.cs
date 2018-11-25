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
        }
    }
}