using System;
using Code.Data;
using Code.Enemy;
using Code.Infrastructure.Services;
using Code.Infrastructure.Services.Input;
using Code.Infrastructure.Services.PersistentProgress;
using Code.Logic;
using UnityEngine;

namespace Code.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        public HeroAnimator HeroAnimator;
        public CharacterController CharacterController;
        private IInputService _input;

        private static int _layerMask;
        private Collider[] _hits = new Collider[3];
        private HeroStats _stats;

        private void Awake()
        {
            _input = AllServices.Container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (_input.MeleeAttackButtonUp()) 
                HeroAnimator.PlayAttack();
        }

        public void OnAttack()
        {
            for (int i = 0; i < Hit(); ++i)
            {
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);

        private Vector3 StartPoint() =>
            new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
    }
}