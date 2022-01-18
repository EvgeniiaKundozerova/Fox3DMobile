using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public Follow Follow;
        public Patrol Patrol;
        public EnemyAnimator EnemyAnimator;
        public NavMeshAgent Agent;

        private bool _isAlerted;

        private void Start()
        {
            TriggerObserver.TriggerEnter += TriggerEnter;

            SwitchFollowOff();
        }

        private void TriggerEnter(Collider obj)
        {
            if (!_isAlerted)
            {
                EnemyAnimator.PlayAlert();
                _isAlerted = true;
            }

            SwitchPatrolOff();
            SwitchFollowOn();
        }

        public void OnAlertStart()
        {
            Agent.updateRotation = false;
        }

        public void OnAlertEnd()
        {
            Agent.updateRotation = true;
        }

        private void SwitchFollowOn() =>
            Follow.enabled = true;

        private void SwitchFollowOff() =>
            Follow.enabled = false;
        
        private void SwitchPatrolOff() =>
            Patrol.enabled = false;
    }
}