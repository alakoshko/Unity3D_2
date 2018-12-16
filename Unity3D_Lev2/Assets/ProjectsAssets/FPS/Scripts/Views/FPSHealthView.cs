using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class FPSHealthView : MonoBehaviour
    {
        private Image _heartImage;
        private PlayerModel _playerModel;

        private void Awake()
        {
            _heartImage = GetComponent<Image>();
            _playerModel = FindObjectOfType<PlayerModel>();
            _playerModel.HealthAmountChanged += OnHealthAmountChanged;
            OnHealthAmountChanged(1);
        }

        private void OnHealthAmountChanged(float amount) { _heartImage.fillAmount = amount; }

        private void OnDestroy()
        {
            _playerModel.HealthAmountChanged -= OnHealthAmountChanged;
        }
    }
}