using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthSystem
{
    public class PlayerHealth : Health
    {
        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);

            Debug.Log("Player Damaged");
        }
    }
}