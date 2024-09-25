using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class ReflectingShieldController : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D circleCollider;
        private float radius;
        private int damageValue;
        private AbilityType abilityType;

        PlayerHealth playerHealth;


        Transform pointerDir;

        Vector3 initialDirection;

        private void OnEnable()
        {
            pointerDir = FindObjectOfType<Pointer_Sys>().transform;
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        public void SetReflectionShieldProperties(ReflectingShieldProperties _reflectingShieldProperties)
        {
            radius = _reflectingShieldProperties.radius;
            abilityType = _reflectingShieldProperties.abilityType;
            damageValue = _reflectingShieldProperties.newDamageValue;

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
                if (!collision.CompareTag("Projectile"))
                {
                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 reverseDirection = (Vector2)Vector3.Normalize(pointerDir.position - collision.transform.position);

                        rb.velocity = reverseDirection.normalized * 5;
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