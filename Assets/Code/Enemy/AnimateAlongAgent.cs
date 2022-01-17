using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    //https://docs.unity3d.com/2019.4/Documentation/Manual/nav-CouplingAnimationAndNavigation.html

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class AnimateAlongAgent : MonoBehaviour
    {
        private const float MinimalVelocity = 0.3f;

        public NavMeshAgent Agent;
        public EnemyAnimator EnemyAnimator;
        public float MaxDistanceToGround = 0.4f;

        private Vector2 _smoothDeltaPosition = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;

        private void Start()
        {
            Agent.updatePosition = false;
        }

        private void Update()
        {
            Vector3 worldDeltaPosition = Agent.nextPosition - transform.position;

            CalculateVelocity(worldDeltaPosition);

            // Pull character towards agent
            if (worldDeltaPosition.magnitude > Agent.radius)
                Agent.nextPosition = transform.position + 0.2f * worldDeltaPosition;
            
            if (ShouldMove())
                EnemyAnimator.Move(_velocity.x, _velocity.y);

            EnemyAnimator.PlayGrounding(IsOnGround());
        }

        private void CalculateVelocity(Vector3 worldDeltaPosition)
        {
            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
                _velocity = _smoothDeltaPosition / Time.deltaTime;
        }

        private void OnAnimatorMove()
        {
            // Update position based on animation movement using navigation surface height
            transform.position = EnemyAnimator.RootPosition();
        }

        private bool ShouldMove() =>
            Agent.velocity.magnitude > MinimalVelocity && Agent.remainingDistance > Agent.radius;

        private bool IsOnGround()
        {
            Debug.DrawRay(transform.position + (Vector3.up * 0.1f), Vector3.down * MaxDistanceToGround, Color.yellow);
            return Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, MaxDistanceToGround);
        }
    }
}