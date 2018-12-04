using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class BaseAmmo : BaseSceneObject, IPoolable
    {
        public abstract void Initialize(Transform firepoint, float force);

        [SerializeField]
        protected float _damage = 20f;


        //IPoolable
        #region
        public abstract string PoolID { get; }
        public abstract int ObjectsCount { get; }

        private void Disable() => gameObject.SetActive(false);
        #endregion
    }
}