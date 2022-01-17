using System;
using System.Collections;
using UnityEngine;

namespace Code.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        public EnemyHealth EnemyHealth;
        public EnemyAnimator EnemyAnimator;
        public Follow Follow;

        public GameObject DeathFx;
        public float DeathFxSpawnHeight;
        private Vector3 _deathFxSpawnPoint;

        public event Action Happened;
        private void Start()
        {
            EnemyHealth.HealthChanged += HealthChanged;
        }

        private void OnDestroy() => 
            EnemyHealth.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (EnemyHealth.Current <= 0)
                Die();
        }

        private void Die()
        {
            EnemyHealth.HealthChanged -= HealthChanged;
            
            EnemyAnimator.PlayDeath();
            
            SwitchFollowOff();
            
            StartCoroutine(DestroyTimer());
            
            Happened?.Invoke();
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(1);
            SpawnDeathFx();
            Destroy(gameObject);
        }

        private void SwitchFollowOff() =>
            Follow.enabled = false;

        private void SpawnDeathFx()
        {
            _deathFxSpawnPoint = new Vector3(transform.position.x, transform.position.y + DeathFxSpawnHeight, transform.position.z);
            Instantiate(DeathFx, _deathFxSpawnPoint, Quaternion.identity);
        }
    }
}