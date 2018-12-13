/**************************************									
	FlockChild v2.3
	Copyright Unluck Software	
 	www.chemicalbliss.com								
***************************************/

using UnityEngine;
using UnityEngine.Video;
using System;
using Random = UnityEngine.Random;

namespace FPS
{
    public class FlockChild : BaseSceneObject, IDamageable
    {
        [HideInInspector]
        public FlockController _spawner;            //Reference to the flock controller that spawned this bird
        //[HideInInspector]
        public float _debugAttackDist;
        public Vector3 _wayPoint;               //Waypoint used to steer towards
        public float _speed;                        //Current speed of bird
        [HideInInspector]
        public bool _dived = true;              //Indicates if this bird has recently performed a dive movement
        [HideInInspector]
        public float _stuckCounter;             //prevents looping around a waypoint by increasing minimum distance to waypoint
        [HideInInspector]
        public float _damping;                      //Damping used for steering (steer speed)
        [HideInInspector]
        public bool _soar = true;               // Indicates if this is soaring
        [HideInInspector]
        public bool _landing;                   // Indicates if bird is landing or sitting idle
        int _lerpCounter;           // Used for smoothing motions like speed and leveling rotation
        [HideInInspector]
        public float _targetSpeed;                  // Max bird speed
        [HideInInspector]
        public bool _move = true;               // Indicates if bird can fly
        public GameObject _model;                   // Reference to bird model
        public Transform _modelT;                   // Reference to bird model transform (caching tranform to avoid any extra getComponent calls)
        [HideInInspector]
        public float _avoidValue;                   //Random value used to check for obstacles. Randomized to lessen uniformed behaviour when avoiding
        [HideInInspector]
        public float _avoidDistance;                //How far from an obstacle this can be before starting to avoid it
        float _soarTimer;
        bool _instantiated;
        static int _updateNextSeed = 0;
        int _updateSeed = -1;
        [HideInInspector]
        public bool _avoid = true;
        public Transform _thisT;

        private Animator m_Animator;
        private Transform _targetTransform;
        private bool _seenTarget;
        public float AttackDistance = 30f;
        public float SearchDistance = 10f;
        public Transform EyesTransform;
        [SerializeField]
        private BaseWeapons _weaponFireBall;

        private void SetTarget(Transform target) => _targetTransform = target;

        public void Start()
        {
            FindRequiredComponents();           //Check if references to transform and model are set (These should be set in the prefab to avoid doind this once a bird is spawned, click "Fill" button in prefab)
            Wander(0.0f);
            SetRandomScale();
            _thisT.position = findWaypoint();

            
            RandomizeStartAnimationFrame();

            InitAvoidanceValues();
            _speed = _spawner._minSpeed;
            _spawner._activeChildren++;
            _instantiated = true;
            if (_spawner._updateDivisor > 1)
            {
                int _updateSeedCap = _spawner._updateDivisor - 1;
                _updateNextSeed++;
                this._updateSeed = _updateNextSeed;
                _updateNextSeed = _updateNextSeed % _updateSeedCap;
            }

            //вызываем дракона на 
            SetTarget(PlayerModel.LocalPlayer.transform);
        }

        public void Awake()
        {
            m_Animator = GetComponent<Animator>();
            //m_Animator.StartPlayback();

            _currentHealth = MaxHealth;
            _weaponFireBall = GetComponentInChildren<BaseWeapons>();

            //TODO: убрать после реализации позиционировани€ цели
            //_seenTarget = false;
            //TODO: убрать после реализации позиционировани€ цели
        }

        public void Update()
        {
            if (_currentHealth <= 0) return;

            //Skip frames
            if (_spawner._updateDivisor <= 1 || _spawner._updateCounter == _updateSeed)
            {
                //SoarTimeLimit();

                #region ƒвижение
                CheckForDistanceToWaypoint();
                RotationBasedOnWaypointOrAvoidance();
                #endregion

                //LimitRotationOfModel();
            }
        }

        public void OnDisable()
        {
            CancelInvoke();
            _spawner._activeChildren--;
        }

        public void OnEnable()
        {
            if (_instantiated)
            {
                _spawner._activeChildren++;

                if (_landing)
                {
                    //_model.GetComponent<Animation>().Play(_spawner._idleAnimation);
                    m_Animator.Play(_spawner._idleAnimation, - 1, Random.Range(0.0f, 1.0f));
                }
                else
                {
                    m_Animator.Play(_spawner._flapAnimation, -1, Random.Range(0.0f, 1.0f));
                    //_model.GetComponent<Animation>().Play(_spawner._flapAnimation);
                }
            }
        }

        public void FindRequiredComponents()
        {
            if (_thisT == null) _thisT = transform;
            if (_model == null) _model = _thisT.Find("Model").gameObject;
            if (_modelT == null) _modelT = _model.transform;
        }

        public void RandomizeStartAnimationFrame()
        {
            //foreach(AnimationState state in _model.GetComponent<Animation>()) {
            //{
            //state.time = Random.value * state.length;
            //}

            //m_Animator.StartPlayback();
            m_Animator.playbackTime = Random.Range(0.0f, 1.0f) * m_Animator.GetCurrentAnimatorStateInfo(0).length;


            //float time;
            //RuntimeAnimatorController ac = m_Animator.runtimeAnimatorController;    //Get Animator controller
            //for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
            //{
            //    time = Random.value * ac.animationClips[i].length;
            //}
        }

        public void InitAvoidanceValues()
        {
            _avoidValue = Random.Range(.3f, .1f);
            if (_spawner._birdAvoidDistanceMax != _spawner._birdAvoidDistanceMin)
            {
                _avoidDistance = Random.Range(_spawner._birdAvoidDistanceMax, _spawner._birdAvoidDistanceMin);
                return;
            }
            _avoidDistance = _spawner._birdAvoidDistanceMin;
        }

        public void SetRandomScale()
        {
            float sc = Random.Range(_spawner._minScale, _spawner._maxScale);
            _thisT.localScale = new Vector3(sc, sc, sc);
        }

        //Soar Timeout - Limits how long a bird can soar
        public void SoarTimeLimit()
        {
            if (this._soar && _spawner._soarMaxTime > 0)
            {
                if (_soarTimer > _spawner._soarMaxTime)
                {
                    this.Flap();
                    _soarTimer = 0.0f;
                }
                else
                {
                    _soarTimer += _spawner._newDelta;
                }
            }
        }

        public void CheckForDistanceToWaypoint()
        {
            //TODO: сделать поиск в каждом кадре Player.LocalPlayer, при нахождении идти только к нему.
            if (!_landing && (_thisT.position - _wayPoint).magnitude < _spawner._waypointDistance + _stuckCounter)
            {
                Wander(0.0f);
                SearchPlayer();
                _stuckCounter = 0.0f;
            }
            else if (!_landing)
            {
                SearchPlayer();
                _stuckCounter += _spawner._newDelta;
            }
            else
            {
                _stuckCounter = 0.0f;
            }
        }

        public void RotationBasedOnWaypointOrAvoidance()
        {
            Vector3 lookit = _wayPoint - _thisT.position;
            Debug.DrawLine(_wayPoint, _thisT.position);
            if (_targetSpeed > -1 && lookit != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(lookit);

                _thisT.rotation = Quaternion.Slerp(_thisT.rotation, rotation, _spawner._newDelta * _damping);
            }

            if (_spawner._childTriggerPos)
            {
                if ((_thisT.position - _spawner._posBuffer).magnitude < 1)
                {
                    _spawner.SetFlockRandomPosition();
                }
            }
            _speed = Mathf.Lerp(_speed, _targetSpeed, _lerpCounter * _spawner._newDelta * .05f);
            _lerpCounter++;
            //Position forward based on object rotation
            if (_move)
            {
                _thisT.position += _thisT.forward * _speed * _spawner._newDelta;
                if (_avoid && _spawner._birdAvoid)
                    Avoidance();
            }
        }

        public bool Avoidance()
        {
            RaycastHit hit = new RaycastHit();
            Vector3 fwd = _modelT.forward;
            bool r = false;
            Quaternion rot = Quaternion.identity;
            Vector3 rotE = Vector3.zero;
            Vector3 pos = Vector3.zero;
            pos = _thisT.position;
            rot = _thisT.rotation;
            rotE = _thisT.rotation.eulerAngles;
            if (Physics.Raycast(_thisT.position, fwd + (_modelT.right * _avoidValue), out hit, _avoidDistance, _spawner._avoidanceMask))
            {
                rotE.y -= _spawner._birdAvoidHorizontalForce * _spawner._newDelta * _damping;
                rot.eulerAngles = rotE;
                _thisT.rotation = rot;
                r = true;
            }
            else if (Physics.Raycast(_thisT.position, fwd + (_modelT.right * -_avoidValue), out hit, _avoidDistance, _spawner._avoidanceMask))
            {
                rotE.y += _spawner._birdAvoidHorizontalForce * _spawner._newDelta * _damping;
                rot.eulerAngles = rotE;
                _thisT.rotation = rot;
                r = true;
            }
            if (_spawner._birdAvoidDown && !this._landing && Physics.Raycast(_thisT.position, -Vector3.up, out hit, _avoidDistance, _spawner._avoidanceMask))
            {
                rotE.x -= _spawner._birdAvoidVerticalForce * _spawner._newDelta * _damping;
                rot.eulerAngles = rotE;
                _thisT.rotation = rot;
                pos.y += _spawner._birdAvoidVerticalForce * _spawner._newDelta * .01f;
                _thisT.position = pos;
                r = true;
            }
            else if (_spawner._birdAvoidUp && !this._landing && Physics.Raycast(_thisT.position, Vector3.up, out hit, _avoidDistance, _spawner._avoidanceMask))
            {
                rotE.x += _spawner._birdAvoidVerticalForce * _spawner._newDelta * _damping;
                rot.eulerAngles = rotE;
                _thisT.rotation = rot;
                pos.y -= _spawner._birdAvoidVerticalForce * _spawner._newDelta * .01f;
                _thisT.position = pos;
                r = true;
            }
            return r;
        }

        public void LimitRotationOfModel()
        {
            Quaternion rot = Quaternion.identity;
            Vector3 rotE = Vector3.zero;
            rot = _modelT.localRotation;
            rotE = rot.eulerAngles;
            if ((_soar && _spawner._flatSoar || _spawner._flatFly && !_soar) && _wayPoint.y > _thisT.position.y || _landing)
            {
                rotE.x = Mathf.LerpAngle(_modelT.localEulerAngles.x, -_thisT.localEulerAngles.x, _lerpCounter * _spawner._newDelta * .75f);
                rot.eulerAngles = rotE;
                _modelT.localRotation = rot;
            }
            else
            {
                rotE.x = Mathf.LerpAngle(_modelT.localEulerAngles.x, 0.0f, _lerpCounter * _spawner._newDelta * .75f);
                rot.eulerAngles = rotE;
                _modelT.localRotation = rot;
            }
        }

        private void SearchPlayer()
        {
            //AIcode from Lesson4
            if (_targetTransform)
            {
                float dist = Vector3.Distance(_thisT.transform.position, _targetTransform.position);
                _debugAttackDist = dist;
                if (dist < AttackDistance)
                {
                    _seenTarget = IsTargetSeen();
                    if (_seenTarget && _weaponFireBall)
                        _weaponFireBall.TryShoot();

                }
                else if (dist < SearchDistance)
                {
                    _seenTarget = IsTargetSeen();
                }
                else
                {
                    _seenTarget = false;
                }

                if (_seenTarget)
                {
                    Debug.DrawLine(EyesTransform.position, _targetTransform.position, Color.red);
                    _wayPoint = _targetTransform.position;
                    //Invoke("SetPlayerAsTarget", delay);
                }
                else
                    Debug.DrawLine(EyesTransform.position, _targetTransform.position, Color.green);
            }
        }

        public void Wander(float delay)
        {
            if (!_landing)
            {
                _damping = Random.Range(_spawner._minDamping, _spawner._maxDamping);
                _targetSpeed = Random.Range(_spawner._minSpeed, _spawner._maxSpeed);
                _lerpCounter = 0;


                //added AIcode from Lesson4
                if (_spawner.UseRandomWP)
                    Invoke("SetRandomMode", delay);
            }
        }

        private bool IsTargetSeen()
        {
            RaycastHit hit;
            if (Physics.Linecast(EyesTransform.position, _targetTransform.position, out hit))
            {
                Debug.DrawLine(EyesTransform.position, hit.point, Color.blue);
                if (hit.transform == _targetTransform)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetRandomMode()
        {
            CancelInvoke("SetRandomMode");
            
            if (!_dived && Random.value < _spawner._soarFrequency)
            {
                Soar();
            }
            else if (!_dived && Random.value < _spawner._diveFrequency)
            {
                Dive();
            }
            else
            {
                Flap();
            }
        }

        public void Flap()
        {
            if (_move)
            {
                //if(this._model != null) _model.GetComponent<Animation>().CrossFade(_spawner._flapAnimation, .5f);
                _soar = false;
                animationSpeed();
                _wayPoint = findWaypoint();
                _dived = false;
            }
        }

        public Vector3 findWaypoint()
        {
            Vector3 t = Vector3.zero;
            if (_seenTarget)
                t = _targetTransform.position;
            else
            {
                t.x = Random.Range(-_spawner._spawnSphere, _spawner._spawnSphere) + _spawner._posBuffer.x;
                t.z = Random.Range(-_spawner._spawnSphereDepth, _spawner._spawnSphereDepth) + _spawner._posBuffer.z;
                t.y = Random.Range(-_spawner._spawnSphereHeight, _spawner._spawnSphereHeight) + _spawner._posBuffer.y;
            }
            return t;
        }

        public void Soar()
        {
            if (_move)
            {
                //_model.GetComponent<Animation>().CrossFade(_spawner._soarAnimation, 1.5f);
                _wayPoint = findWaypoint();
                _soar = true;
            }
        }

        public void Dive()
        {
            if (_spawner._soarAnimation != null)
            {
                //_model.GetComponent<Animation>().CrossFade(_spawner._soarAnimation, 1.5f);
            }
            else
            {
                //foreach (AnimationState state in _model.GetComponent<Animation>())

                if (_thisT.position.y < _wayPoint.y + 25)
                    m_Animator.speed = 0.1f;
                //foreach (AnimationState state in _model.GetComponent<Animation>())
                //{
                //    if (_thisT.position.y < _wayPoint.y + 25)
                //    {
                //        state.speed = 0.1f;
                //    }
                //}
            }
            _wayPoint = findWaypoint();
            _wayPoint.y -= _spawner._diveValue;
            _dived = true;
        }

        public void animationSpeed()
        {
            //foreach (AnimationState state in _model.GetComponent<Animation>())
            //{
                if (!_dived && !_landing)
                {
                    //state.speed = Random.Range(_spawner._minAnimationSpeed, _spawner._maxAnimationSpeed);
                    m_Animator.speed = Random.Range(_spawner._minAnimationSpeed, _spawner._maxAnimationSpeed);
                }
                else
                {
                    m_Animator.speed = _spawner._maxAnimationSpeed;
                }
            //}
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
            m_Animator.Play(_spawner._damageAnimation);



            Debug.Log($"Dragon current health: {_currentHealth}");

            if (CurrentHealth <= 0) Die();
        }

        public void Die()
        {
            Collider.enabled = false;
            var rb = GetComponent<Transform>().gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = 1000;
            //rb.AddForce(Vector3.up * Random.Range(10f, 30f), ForceMode.Impulse);

            m_Animator?.Play(_spawner._deadAnimation);

            _spawner.DieChild(this);
            //Destroy(gameObject, 5f);
        }
        #endregion
    }
}