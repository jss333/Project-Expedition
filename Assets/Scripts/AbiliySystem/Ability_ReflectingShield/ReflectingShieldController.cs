using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class ReflectingShieldController : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D circleCollider;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private float radius;
        private float reverseSpeed;
        private int damageValue;
        private AbilityType abilityType;

        PlayerHealth playerHealth;
        Transform pointerDir;
        Vector3 initialDirection;
        //private AudioClip rechargeClip;
        private EventReference chargeSoundEvent;

        private void OnEnable()
        {
            pointerDir = FindObjectOfType<Pointer_Sys>().transform;
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        private void OnDestroy()
        {
            RemoveAbilityVisualFromScene();
        }

        public void SetReflectionShieldProperties(ReflectingShieldProperties reflectingShieldProperties)
        {
            radius = reflectingShieldProperties.radius;
            abilityType = reflectingShieldProperties.abilityType;
            damageValue = reflectingShieldProperties.newDamageValue;
            reverseSpeed = reflectingShieldProperties.reverseSpeed;
            chargeSoundEvent = reflectingShieldProperties.chargeSoundEvent;

            SetUpRadiusDependencies();
        }

        public void RemoveAbilityVisualFromScene()
        {
            if (gameObject != null)
            {
                spriteRenderer.enabled = false;
                circleCollider.enabled = false;
            }
            //Destroy(gameObject);
        }

        public void RemoveAbilityGameObjectFromScene()
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        public void OnRechargeAbility()
        {
            StartCoroutine(PlaySoundAndDestroyObject());
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
                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 reverseDirection = (Vector2)Vector3.Normalize(pointerDir.position - collision.transform.position);
                        rb.velocity = reverseDirection.normalized * reverseSpeed;
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

        IEnumerator PlaySoundAndDestroyObject()
        {
            //AudioManagerNoMixers.Singleton.PlaySFXByName("ShieldCharged");
            AudioManagerNoMixers.Singleton.PlayOneShot(chargeSoundEvent, this.transform.position);

            yield return new WaitForSeconds(1.2f);

            RemoveAbilityGameObjectFromScene();
        }

    }
}