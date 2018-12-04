using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class FlashLightModel : MonoBehaviour
    {
        public event Action<float> ChargeAmountChanged;
        public event Action<bool> FlashLightStateChanged;
        public float ChargeAmount { get; private set; }

        [SerializeField]
        private float _drainMult = 3f;
        [SerializeField]
        private float _rechargeTime = 60f;

        private Light _light;

        public bool IsOn { get { return _light.enabled; } }

        private void Awake() {
            _light = GetComponent<Light>();
            ChargeAmount = 1f;
        }

        private void OnEnable()
        {
            StartCoroutine(ChangeFill());
        }

        private void OnDisable()
        {
            StopCoroutine(ChangeFill());
        }

        public void On() {
            if (ChargeAmount <= 0.3f) return;

            _light.enabled = true;
            if (FlashLightStateChanged != null) FlashLightStateChanged.Invoke(true);
        }
        public void Off() {
            _light.enabled = false;
            if (FlashLightStateChanged != null) FlashLightStateChanged.Invoke(false);
        }

        private const float chargeDelay = 1f;
        private IEnumerator ChangeFill()
        {
            while (true)
            {
                yield return new WaitForSeconds(chargeDelay);
                if (IsOn)
                {
                    ChargeAmount = Mathf.Clamp01(ChargeAmount - 1f / (Mathf.Max(0.01f, _rechargeTime * _drainMult) * chargeDelay));
                    if (ChargeAmount <= 0f) Off();
                }
                else
                {
                    ChargeAmount = Mathf.Clamp01(ChargeAmount + 1f / (Mathf.Max(0.01f, _rechargeTime) * chargeDelay));
                }
                if (ChargeAmountChanged != null) ChargeAmountChanged.Invoke(ChargeAmount);
            }
        }
    }
}