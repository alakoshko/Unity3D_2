using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class FlashLightView : MonoBehaviour
    {
        [SerializeField]
        private Color _onColor;
        [SerializeField]
        private Color _offColor;

        private Image _image;
        private FlashLightModel _flashLightModel;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _flashLightModel = FindObjectOfType<FlashLightModel>();
            _flashLightModel.FlashLightStateChanged += OnFlashLightStateChanged;
            OnFlashLightStateChanged(false);
        }

        private void OnDestroy()
        {
            _flashLightModel.FlashLightStateChanged -= OnFlashLightStateChanged;
        }

        private void OnFlashLightStateChanged(bool state)
        {
            _image.color = state ? _onColor : _offColor;
        }
    }

}