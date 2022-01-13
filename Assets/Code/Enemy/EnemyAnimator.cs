using System;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Attack = Animator.StringToHash("Attack_1");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int VelocityX = Animator.StringToHash("VelX");
        private static readonly int VelocityY = Animator.StringToHash("VelY");
        private static readonly int OnGround = Animator.StringToHash("OnGround");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayHit() => _animator.SetTrigger(Hit);
        public void PlayDeath() => _animator.SetTrigger(Death);

        public void Move(float velocityX, float velocityY)
        {
            _animator.SetBool(IsMoving, true);
            _animator.SetFloat(VelocityX, velocityX);
            _animator.SetFloat(VelocityY, velocityY);
        }

        public Vector3 RootPosition() => 
            _animator.rootPosition;

        public void StopMoving() => _animator.SetBool(IsMoving, false);

        public void PlayAttack() => _animator.SetTrigger(Attack);

        public void PlayGrounding(bool onGround) => _animator.SetBool(OnGround, onGround);
    }
}