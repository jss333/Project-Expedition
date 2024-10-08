using AbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingProjectileShieldController : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float radius;
    private Vector3 initialDirection;
    private AudioClip rechargeClip;

    private void OnDestroy()
    {
        RemoveAbilityVisualFromScene();
    }

    public void SetReflectionShieldProperties(ReflectingShieldProperties reflectingShieldProperties)
    {
        radius = reflectingShieldProperties.radius;
        rechargeClip = reflectingShieldProperties.chargeClip;

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
                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator PlaySoundAndDestroyObject()
    {
        audioSource.PlayOneShot(rechargeClip);


        yield return new WaitForSeconds(1.2f);

        RemoveAbilityGameObjectFromScene();
    }
}
