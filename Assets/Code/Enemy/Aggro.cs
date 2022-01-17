using System.Collections;
using UnityEngine;

namespace Code.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public Follow Follow;
        public EnemyAnimator EnemyAnimator;

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
            
            SwitchFollowOn();
        }

        private void SwitchFollowOn() =>
            Follow.enabled = true;

        private void SwitchFollowOff() =>
            Follow.enabled = false;
    }
}