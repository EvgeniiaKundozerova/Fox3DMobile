using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PatrolWithSpawnSpots : Patrol
    {
        private const float MinimalDistance = 0.5f;
        
        public NavMeshAgent Agent;
        public float Radius;

        private Vector3 _startPoint;
        private Vector3 _pointToGo;
        private int _layerMask;
        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Default");
            _startPoint = Agent.transform.position;
        }
        
        private void Update()
        {
            if (!Agent.pathPending && Agent.remainingDistance < MinimalDistance)
                GoToPoint();
        }
        
        private void GoToPoint()
        {
            SpawnRandomPoint();
            
            Agent.SetDestination(_pointToGo);
        }

        private void SpawnRandomPoint()
        {
            float randomZ = Random.Range(-Radius, Radius);
            float randomX = Random.Range(-Radius, Radius);
            
            Vector3 nextPoint = new Vector3(_startPoint.x + randomX, _startPoint.y + 10f,_startPoint.z + randomZ);
            
            if (Physics.Raycast(nextPoint, -transform.up, out RaycastHit hit, 20f, _layerMask))
            {
                _pointToGo = hit.point;
                Debug.DrawRay(_pointToGo, Vector3.up, Color.white, 10.0f);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
#endif
    }
}