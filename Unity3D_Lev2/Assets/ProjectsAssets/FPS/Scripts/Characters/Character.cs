using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Video;
using UnityEngine.AI;

namespace FPS
{
    public class Character : BaseSceneObject, IDamageable
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

        public Transform EyesTransform;
        public float SearchDistance = 30f;
        public float AttackDistance = 10f;

        public bool UseRandomWP;
        public float MaxRandomWPRaius;
        private Vector3 _randomPos;
        private WayPoint[] _wayPoints;
        private int _currentWP;

        private bool _seenTrgt;
        private Transform _targetTransform;
        private NavMeshAgent _agent;

        private BaseWeapons _weapon;

        public void SetTarget(Transform target) => _targetTransform = target;

        protected override void Awake()
        {
            base.Awake();

            _currentHealth = _maxHealth;
            m_Animator = GetComponent<Animator>();
            m_Animator?.Play(_animatorWait);

            Initialize();
        }
        private void Start()
        {
            SetTarget(PlayerModel.LocalPlayer.transform);
        }

        public void Initialize()
        {
            _agent = GetComponent<NavMeshAgent>();
            if (UseRandomWP)
                _randomPos = GenerateRandomWP();

            _weapon = GetComponentInChildren<BaseWeapons>();
        }

        private void Update()
        {
            if (_currentHealth <= 0) return;
            
            //TODO: delete this
            _seenTrgt = false;

            if (_targetTransform)
            {
                float dist = Vector3.Distance(transform.position, _targetTransform.position);
                if(dist < AttackDistance)
                {
                    if (IsTargetSeen())
                    {
                        _agent.SetDestination(_targetTransform.position);
                        if (_weapon)
                        {
                            _weapon.TryShoot();
                            m_Animator?.Play(_animatorAttack);
                        }
                    }
                }
                else if(dist < SearchDistance)
                {
                    if (IsTargetSeen())
                    {
                        _agent.SetDestination(_targetTransform.position);
                        m_Animator?.Play(_animatorMove);
                    }
                }
                else
                {
                    _seenTrgt = false;
                }
            }

            if (_seenTrgt) return;
            

            if (UseRandomWP)
            {
                //задаем цель 
                _agent.SetDestination(_randomPos);
                //дошел ли до позиции наш бот
                if (!_agent.hasPath || _agent.remainingDistance >= MaxRandomWPRaius * 2)
                    _randomPos = GenerateRandomWP();

            }
        }

        private Vector3 GenerateRandomWP()
        {
            var randomPos = Random.insideUnitSphere * MaxRandomWPRaius;
            //randomPos.y = transform.position.y;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position + randomPos, out hit, MaxRandomWPRaius * 1.5f, NavMesh.AllAreas))
                return hit.position;
            else
                return transform.position;
        }

        private bool IsTargetSeen()
        {
            RaycastHit hit;
            if (Physics.Linecast(EyesTransform.position, _targetTransform.position, out hit)) {
                if (hit.transform == _targetTransform)
                {
                    Debug.DrawLine(EyesTransform.position, hit.point, Color.red);
                    return true;
                }
            }
            Debug.DrawLine(EyesTransform.position, hit.point, Color.green);
            return false;
        }

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
            //m_Animator?.Play(_animatorDamage);
            m_Animator.SetBool(_animatorDamage, true);
            m_Animator.SetBool(_animatorWait, true);
            
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