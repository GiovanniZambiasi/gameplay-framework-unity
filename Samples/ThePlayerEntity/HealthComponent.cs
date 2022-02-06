using UnityEngine;

namespace MiddleMast.GameplayFramework.Samples.ThePlayerEntity
{
    public class HealthComponent : MonoBehaviour
    {
        public event global::System.Action OnDamageTaken;

        private float _health = 100f;
        private float _maxHealth = 100f;

        public float HealthRatio => _health / _maxHealth;

        public void TakeDamage(float damage)
        {
            _health -= damage;

            OnDamageTaken?.Invoke();
        }
    }
}
