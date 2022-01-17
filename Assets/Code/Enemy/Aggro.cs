using System.Collections;
using UnityEngine;

namespace Code.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public Follow Follow;
        
        private Coroutine _aggroCoroutine;
        private bool _hasAggroTarget;

        private void Start()
        {
            TriggerObserver.TriggerEnter += TriggerEnter;

            SwitchFollowOff();
        }

        private void TriggerEnter(Collider obj)
        {
            SwitchFollowOn();
        }


        private void SwitchFollowOn() =>
            Follow.enabled = true;

        private void SwitchFollowOff() =>
            Follow.enabled = false;
    }
}