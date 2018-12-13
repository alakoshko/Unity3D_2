using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Main : MonoBehaviour
    {
        public static Main Instance { get; private set; }
        
        public InputController InputController { get; private set; }
        public FlashLightController FlashLightController { get; private set; }
        public WeaponsController WeaponsController { get; private set; }
        public CastleDoorController CastleDoorController { get; private set; }
        public TeammateController TeammateController { get; private set; }

        private void Awake()
        {
            if (Instance) DestroyImmediate(gameObject);
            else Instance = this;

            //UnityEngine.EventSystems.EventSystem.current
        }

        // Use this for initialization
        void Start()
        {
            InputController = gameObject.AddComponent<InputController>();
            FlashLightController = gameObject.AddComponent<FlashLightController>();
            WeaponsController = gameObject.AddComponent<WeaponsController>();
            CastleDoorController = gameObject.AddComponent<CastleDoorController>();
            TeammateController = gameObject.AddComponent<TeammateController>();
        }

    }
}