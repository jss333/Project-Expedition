using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class ReversingDirectionShieldController : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D circleCollider;
        private float radius;
        private float reverseSpeed;
        private int damageValue;
        private AbilityType abilityType;

        Vector3 initialDirection;


        private void OnDestroy()
        {
            RemoveAbilityVisualFromScene();
        }

        public void SetReflectionShieldProperties(ReflectingShieldProperties _reflectingShieldProperties)
        {
            radius = _reflectingShieldProperties.radius;
            abilityType = _reflectingShieldProperties.abilityType;
            damageValue = _reflectingShieldProperties.newDamageValue;
            reverseSpeed = _reflectingShieldProperties.reverseSpeed;

            SetUpRadiusDependencies();
        }

        public void RemoveAbilityVisualFromScene()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }

        private void SetUpRadiusDependencies()
        {
            SetCollisionRadius();

            SetVisualRadius();
        }

        private void SetCollisionRadius()
        {
            circleCollider.radius = radius;
        }

        private void SetVisualRadius()
        {
            transform.localScale = Vector3.one * radius;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null)
            {
                if (collision.CompareTag("Projectile"))
                {
                    PlayerProjectile playerProjectile = collision.GetComponent<PlayerProjectile>();
                    if (playerProjectile != null) return;

                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = rb.velocity * -reverseSpeed;
                    }

                    BossOrb bossOrb = collision.GetComponent<BossOrb>();
                    if (bossOrb != null)
                    {
                        bossOrb.SetCanDamageBossAndMinions(true);
                        bossOrb.SetNewDamagevalue(damageValue);
                    }
                }
            }
        }
    }
}