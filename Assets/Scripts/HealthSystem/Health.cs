using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthSystem
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] protected int maxHealth;
        protected int currentHealth;
        protected bool canBeDamaged;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(int damageAmount)
        {
            if (!canBeDamaged) return;

            currentHealth -= damageAmount;
        }
    }
}