using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

namespace FPS
{
    public class TeammateModel : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private ThirdPersonCharacter _character;

        [SerializeField]
        private float _followPlayerStopDistance = 1.5f;
        private float _oldStoppingDistance;

        private bool _followPlayer;

        private void Start()
        {
            //_agent = GetComponent<NavMeshAgent>();
            _agent = GetComponentInChildren<NavMeshAgent>();
            _character = GetComponent<ThirdPersonCharacter>();

            _oldStoppingDistance = _agent.stoppingDistance;

            _agent.updateRotation = false;
            _agent.updatePosition = true;
        }

        private void Update() {
            if (_followPlayer && PlayerModel.LocalPlayer != null)
                _agent.SetDestination(PlayerModel.LocalPlayer.transform.position);

            if (_agent.remainingDistance > _agent.stoppingDistance)
                _character.Move(_agent.desiredVelocity, false, false);
            else
                _character.Move(Vector3.zero, false, false);

        }

        public void SwitchFollow()
        {
            _followPlayer = !_followPlayer;

            _agent.stoppingDistance = _followPlayer? _followPlayerStopDistance : _oldStoppingDistance;
        }

        public void SetDestination(Vector3 pos)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 50f, -1))
            {
                _followPlayer = false;  
                _agent.SetDestination(hit.position);
            }
            else Debug.Log("Wrong position");
            //_agent.SetDestination(pos);
        }

    }
}