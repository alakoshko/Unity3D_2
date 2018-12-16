using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace FPS
{
    public class PlayerModel : BaseSceneObject, IDamageable
    {
        public string PlayerName = "Player01";

        public static PlayerModel LocalPlayer { get; private set; }

        public BaseWeapons[] Weapons;
        //public Image ImgMaxHealth;
        public event Action<float> HealthAmountChanged;
        public event Action<bool> HealthStateChanged;

        protected override void Awake()
        {
            base.Awake();

            if (LocalPlayer) DestroyImmediate(gameObject);
            else LocalPlayer = this;

            _currentHealth = _maxHealth;

            if (Weapons == null || Weapons.Length == 0) 
                Weapons = GetComponentsInChildren<BaseWeapons>(true);
        }

        //private void Update()
        //{
        //    ImgMaxHealth.fillAmount = _currentHealth / _maxHealth;
        //}
        //private void OnEnable()
        //{
        //    StartCoroutine(ChangeHealth());
        //}
        //private void OnDisable()
        //{
        //    StopCoroutine(ChangeHealth());
        //}

        //private const float chargeDelay = 1f;
        //private IEnumerator ChangeHealth()
        //{
        //    while (true)
        //    {
        //        yield return new WaitForSeconds(chargeDelay);
        //        ApplyDamage();
        //        if (ChargeAmount <= 0f) Die();
             
        //        if (HealthAmountChanged != null) HealthAmountChanged.Invoke(CurrentHealth);
        //    }
        //}

        #region IDamageable implementation
        [SerializeField]
        private float _maxHealth;
        public float MaxHealth => _maxHealth;

        private float _currentHealth;
        public float CurrentHealth => _currentHealth;


        public void ApplyDamage(float damage)
        {
            if (CurrentHealth <= 0) return;
            _currentHealth -= damage;

            Debug.Log($"Current health: {_currentHealth}");

            if (CurrentHealth <= 0) Die();

            if (HealthAmountChanged != null) HealthAmountChanged.Invoke( _currentHealth / _maxHealth );
        }


        public void Die()
        {
            Collider.enabled = false;
            var rb = GetComponent<Transform>().gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = _maxHealth / 2;
            rb.AddForce(Vector3.up * Random.Range(10f, 30f), ForceMode.Impulse);

            //m_Animator?.Play(_animatorDead);


            gameObject.GetComponentInChildren<Camera>().enabled = false;
            //Destroy(gameObject, 5f);
        }
        #endregion

    }
}