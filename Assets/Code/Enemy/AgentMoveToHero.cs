using System;
using Code.Infrastructure.Factory;
using Code.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    [RequireComponent(typeof(EnemyAnimator), typeof(NavMeshAgent))]
    public class AgentMoveToHero : Follow
    {
        private const float MinimalDistance = 3f;

        public NavMeshAgent Agent;
        public EnemyAnimator EnemyAnimator;

        private Transform _heroTransform;
        private IGameFactory _gameFactory;

        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            if (_gameFactory.HeroGameObject != null)
                InitializeHeroTransform();
            else
                _gameFactory.HeroCreated += HeroCreated;
        }

        private void Update()
        {
            if (Initialized() && HeroNotReached())
                Agent.destination = _heroTransform.position;
            
            EnemyAnimator.SetDistanceToTarget(DistanceToTarget());
        }

        private void InitializeHeroTransform() => 
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private void HeroCreated() => 
            InitializeHeroTransform();

        private bool Initialized() => 
            _heroTransform != null;

        private bool HeroNotReached() =>
            DistanceToTarget() >= MinimalDistance;

        private float DistanceToTarget() => 
            Vector3.Distance(Agent.transform.position, _heroTransform.position);
    }
}