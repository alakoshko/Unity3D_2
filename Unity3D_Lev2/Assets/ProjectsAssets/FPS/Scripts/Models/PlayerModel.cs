using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FPS
{
    public class PlayerModel : BaseSceneObject
    {
        public static PlayerModel LocalPlayer { get; private set; }

        public BaseWeapons[] Weapons;

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