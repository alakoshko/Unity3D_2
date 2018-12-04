using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPS
{
    public class TeammateView : BaseSceneObject
    {
        private TeammateModel _teammateModel;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            //ждем инициализаци всех персонажей с помощью карутина
            yield return new WaitWhile(() => Main.Instance == null);

            _teammateModel = GetComponentInParent<TeammateModel>();
            TeammateController.OnTeammateSelected += OnViewTeammateSelected;

            IsVisible = false;
        }

        private void OnDestroy()
        {
            TeammateController.OnTeammateSelected -= OnViewTeammateSelected;
        }

        private void OnViewTeammateSelected(TeammateModel teammate)
        {
            IsVisible = teammate == _teammateModel; 
        }
        
    }
}