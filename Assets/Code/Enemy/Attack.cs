using System;
using System.Linq;
using Code.Infrastructure.Factory;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator EnemyAnimator;
        public float AttackCooldown = 3f;
        public float Cleavage = 0.5f;
        public float EffectiveDistance = 0.5f;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;
        private Collider[] _hits = new Collider[1];
        
        private float _attackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private bool _attackIsActive;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _gameFactory.HeroCreated += OnHeroCreated;
        }

        private void Update()
        {
            if (CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1);
            }
        }

        private void OnAttackEnded() => 
            _isAttacking = false;

        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

        private void StartAttack()
        {
            EnemyAnimator.PlayAttack();
            _isAttacking = true;
        }

        private bool CanAttack() =>
            !_isAttacking && _attackIsActive;

        private void OnHeroCreated() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;
    }
}