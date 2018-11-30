using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class BaseSceneObject : MonoBehaviour
    {
        protected int _layer;
        public int Layer
        {
            get
            {
                return _layer;
            }
            set
            {
                _layer = value;
                SetLayers(transform, _layer);
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                name = _name;
            }
        }


        protected bool _isVisible;
        public bool IsVisible
        {
            get
            {
                if (!_renderer)
                    return false;

                return _renderer.enabled;
            }
            set
            {
                _isVisible = value;
                SetVisibility(transform, _isVisible);
            }
        }

        protected Collider _collider;
        public Collider Collider
        {
            get
            {
                if (!_collider) _collider = GetComponent<Collider>();
                return _collider;
            }
        }

        protected Rigidbody _rigidbody;
        public Rigidbody Rigidbody
        {
            get
            {
                return _rigidbody;
            }
        }
        protected Material _material;
        public Material Material
        {
            get
            {
                return _material;
            }
        }

        protected GameObject _gameobject;
        public GameObject GameObject
        {
            get
            {
                return _gameobject;
            }
        }

        protected Renderer _renderer;
        protected Color _color;
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                _material.color = _color;
            }
        }

        protected virtual void Awake()
        {
            _gameobject = GetComponent<GameObject>();
            _rigidbody = GetComponent<Rigidbody>();
            Name = name;
            _layer = gameObject.layer;

            _renderer = GetComponent<Renderer>();
            if (_renderer)
                _material = _renderer.material;
            if (_material)
                _color = _material.color;
        }

        private void SetLayers(Transform objTransform, int layer)
        {
            objTransform.gameObject.layer = layer;
            foreach (Transform c in objTransform)
            {
                SetLayers(c, layer);
            }
        }

        private void SetVisibility(Transform objTransform, bool visible)
        {
            var rend = objTransform.GetComponent<Renderer>();
            if (rend)
                rend.enabled = visible;

            foreach (var r in GetComponentsInChildren<Renderer>(true))
                r.enabled = visible;
        }
    }
}

