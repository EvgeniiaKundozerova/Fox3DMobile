using System.Collections;
using UnityEngine;

namespace Code.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        public HeroHealth HeroHealth;
        public HeroMove HeroMove;
        public HeroAttack HeroAttack;
        public HeroAnimator HeroAnimator;
        public GameObject DeathFx;

        public float DestroyTime;
        
        private bool _isDead;

        private void Start() =>
            HeroHealth.HealthChanged += HealthChange;

        private void OnDestroy() =>
            HeroHealth.HealthChanged -= HealthChange;

        private void HealthChange()
        {
            if (!_isDead && HeroHealth.Current <= 0)
                Die();
        }

        private void Die()
        {
            _isDead = true;
            
            HeroMove.enabled = false;
            HeroAttack.enabled = false;
            HeroAnimator.PlayDeath();

            StartCoroutine(DestroyTimer());
        }
        
        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(DestroyTime);
            Instantiate(DeathFx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}