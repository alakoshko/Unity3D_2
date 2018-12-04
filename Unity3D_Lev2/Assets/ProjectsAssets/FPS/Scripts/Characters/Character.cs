using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Video;

namespace FPS
{
    public class Character : BaseSceneObject, IDamageable
    {
        [SerializeField]
        private float _maxHealth;
        public float MaxHealth => _maxHealth;

        private float _currentHealth;
        public float CurrentHealth => _currentHealth;

        private Animator m_Animator;
        [SerializeField]
        private string _animatorDamage;
        [SerializeField]
        private string _animatorAttack;
        [SerializeField]
        private string _animatorDead;
        [SerializeField]
        private string _animatorMove;
        [SerializeField]
        private string _animatorWait;

        public void Start()
        {
            _currentHealth = _maxHealth;
            m_Animator = GetComponent<Animator>();
        }

        public void ApplyDamage(float damage)
        {
            if (CurrentHealth <= 0) return;
            _currentHealth -= damage;
            m_Animator?.Play(_animatorDamage);

            Debug.Log($"Current health: {_currentHealth}");

            if (CurrentHealth <= 0) Die();
        }

        public void Die()
        {
            Collider.enabled = false;
            var rb = GetComponent<Transform>().gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = _maxHealth / 2;
            rb.AddForce(Vector3.up * Random.Range(10f, 30f), ForceMode.Impulse);

            m_Animator?.Play(_animatorDead);
            m_Animator?.Play(_animatorDead);
            Destroy(gameObject, 5f);
        }
    }
}