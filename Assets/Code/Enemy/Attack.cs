using System.Linq;
using Code.Infrastructure.Factory;
using Code.Infrastructure.Services;
using Code.Logic;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator EnemyAnimator;
        public NavMeshAgent Agent;
        public float AttackCooldown = 3f;
        public float Cleavage = 0.5f;
        public float EffectiveDistance = 0.5f;
        public float Damage = 10f;

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

        private void OnAttackStarted()
        {
            Agent.updateRotation = false;
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1);
                hit.transform.GetComponent<IHealth>().TakeDamage(Damage); 
            }
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
            Agent.updateRotation = true;
        }

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