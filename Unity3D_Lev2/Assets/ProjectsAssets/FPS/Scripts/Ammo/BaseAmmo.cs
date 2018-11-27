using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class BaseAmmo : BaseSceneObject
    {
        public abstract void Initialize(Transform firepoint, float force);   
    }
}