using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Main : MonoBehaviour
    {
        public static Main Instance { get; private set; }
        
        public KeyboardInputController KeyboardInputController { get; private set; }
        public FlashLightController FlashLightController { get; private set; }
        public WeaponsController WeaponsController { get; private set; }

        private void Awake()
        {
            if (Instance) DestroyImmediate(gameObject);
            else Instance = this;

            //UnityEngine.EventSystems.EventSystem.current
        }

        // Use this for initialization
        void Start()
        {
            KeyboardInputController = gameObject.AddComponent<KeyboardInputController>();
            FlashLightController = gameObject.AddComponent<FlashLightController>();
            WeaponsController = gameObject.AddComponent<WeaponsController>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}