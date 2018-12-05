using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FPS
{
    [RequireComponent(typeof(FlyingCharacterController))]
    public class FlyingCharacterAi : BaseSceneObject, IDamageable
    {
        #region Animator clips name
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
        #endregion

        #region AI
        // This script represents an AI 'pilot' capable of flying the plane towards a designated target.
        // It sends the equivalent of the inputs that a user would send to the Aeroplane controller.
        [SerializeField] private float m_RollSensitivity = .2f;         // How sensitively the AI applies the roll controls
        [SerializeField] private float m_PitchSensitivity = .5f;        // How sensitively the AI applies the pitch controls
        [SerializeField] private float m_LateralWanderDistance = 5;     // The amount that the plane can wander by when heading for a target
        [SerializeField] private float m_LateralWanderSpeed = 0.11f;    // The speed at which the plane will wander laterally
        [SerializeField] private float m_MaxClimbAngle = 45;            // The maximum angle that the AI will attempt to make plane can climb at
        [SerializeField] private float m_MaxRollAngle = 45;             // The maximum angle that the AI will attempt to u
        [SerializeField] private float m_SpeedEffect = 0.01f;           // This increases the effect of the controls based on the plane's speed.
        [SerializeField] private float m_TakeoffHeight = 20;            // the AI will fly straight and only pitch upwards until reaching this height
        [SerializeField] private Transform m_Target;                    // the target to fly towards

        private FlyingCharacterController m_FlyingCharacterController;  // The aeroplane controller that is used to move the plane
        private float m_RandomPerlin;                       // Used for generating random point on perlin noise so that the plane will wander off path slightly
        private bool m_TakenOff;                            // Has the plane taken off yet


        // setup script properties
        private void Awake()
        {
            // get the reference to the aeroplane controller, so we can send move input to it and read its current state.
            m_FlyingCharacterController = GetComponent<FlyingCharacterController>();

            // pick a random perlin starting point for lateral wandering
            m_RandomPerlin = Random.Range(0f, 100f);

            //Turn On animation
            m_Animator = GetComponent<Animator>();
            m_Animator?.Play(_animatorWait);
            _currentHealth = _maxHealth;
        }


        // reset the object to sensible values
        public void Reset()
        {
            m_TakenOff = false;
        }


        // fixed update is called in time with the physics system update
        private void FixedUpdate()
        {
            if (m_Target != null)
            {
                // make the plane wander from the path, useful for making the AI seem more human, less robotic.
                Vector3 targetPos = m_Target.position +
                                    transform.right *
                                    (Mathf.PerlinNoise(Time.time * m_LateralWanderSpeed, m_RandomPerlin) * 2 - 1) *
                                    m_LateralWanderDistance;

                // adjust the yaw and pitch towards the target
                Vector3 localTarget = transform.InverseTransformPoint(targetPos);
                float targetAngleYaw = Mathf.Atan2(localTarget.x, localTarget.z);
                float targetAnglePitch = -Mathf.Atan2(localTarget.y, localTarget.z);


                // Set the target for the planes pitch, we check later that this has not passed the maximum threshold
                targetAnglePitch = Mathf.Clamp(targetAnglePitch, -m_MaxClimbAngle * Mathf.Deg2Rad,
                                               m_MaxClimbAngle * Mathf.Deg2Rad);

                // calculate the difference between current pitch and desired pitch
                float changePitch = targetAnglePitch - m_FlyingCharacterController.PitchAngle;

                // AI always applies gentle forward throttle
                const float throttleInput = 0.5f;

                // AI applies elevator control (pitch, rotation around x) to reach the target angle
                float pitchInput = changePitch * m_PitchSensitivity;

                // clamp the planes roll
                float desiredRoll = Mathf.Clamp(targetAngleYaw, -m_MaxRollAngle * Mathf.Deg2Rad, m_MaxRollAngle * Mathf.Deg2Rad);
                float yawInput = 0;
                float rollInput = 0;
                if (!m_TakenOff)
                {
                    // If the planes altitude is above m_TakeoffHeight we class this as taken off
                    if (m_FlyingCharacterController.Altitude > m_TakeoffHeight)
                    {
                        m_TakenOff = true;
                    }
                }
                else
                {
                    // now we have taken off to a safe height, we can use the rudder and ailerons to yaw and roll
                    yawInput = targetAngleYaw;
                    rollInput = -(m_FlyingCharacterController.RollAngle - desiredRoll) * m_RollSensitivity;
                }

                // adjust how fast the AI is changing the controls based on the speed. Faster speed = faster on the controls.
                float currentSpeedEffect = 1 + (m_FlyingCharacterController.ForwardSpeed * m_SpeedEffect);
                rollInput *= currentSpeedEffect;
                pitchInput *= currentSpeedEffect;
                yawInput *= currentSpeedEffect;

                // pass the current input to the plane (false = because AI never uses air brakes!)
                m_FlyingCharacterController.Move(rollInput, pitchInput, yawInput, throttleInput, false);
            }
            else
            {
                // no target set, send zeroed input to the planeW
                m_FlyingCharacterController.Move(0, 0, 0, 0, false);
            }
        }


        // allows other scripts to set the plane's target
        public void SetTarget(Transform target)
        {
            m_Target = target;
        }

        #endregion

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
            //m_Animator?.SetBool(_animatorWait, false);
            m_Animator?.Play(_animatorDamage);
            //m_Animator?.SetBool(_animatorWait, true);
            //m_Animator.StartPlayback();



            //Опять не запускает анимацию, почему?
            //m_Animator?.Play(_animatorWait);

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
            //Опять не запускает анимацию, почему?
            //m_Animator?.Play(_animatorWait);

            Destroy(gameObject, 5f);
        }
        #endregion
    }
}
