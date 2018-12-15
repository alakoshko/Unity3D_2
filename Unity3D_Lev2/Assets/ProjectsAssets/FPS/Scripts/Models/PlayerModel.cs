using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FPS
{
    public class PlayerModel : BaseSceneObject
    {
        public string PlayerName = "Player01";

        public static PlayerModel LocalPlayer { get; private set; }

        public BaseWeapons[] Weapons;

        public float Health = 98.7f;

        protected override void Awake()
        {
            base.Awake();

            if (LocalPlayer) DestroyImmediate(gameObject);
            else LocalPlayer = this;

            if(Weapons == null || Weapons.Length == 0) 
                Weapons = GetComponentsInChildren<BaseWeapons>(true);
        }
        
    }
}