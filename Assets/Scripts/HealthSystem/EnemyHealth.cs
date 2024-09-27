using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthSystem
{
    public class EnemyHealth : Health
    {
        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);

            Debug.Log("Enemy Damaged");
        }
    }
}