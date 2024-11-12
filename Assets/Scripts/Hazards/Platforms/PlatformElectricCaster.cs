using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformElectricCaster : MonoBehaviour
{

    [SerializeField] private int damage;
    [SerializeField] private float timeToCastDamage;

    [SerializeField] private float damageTimer;
    [SerializeField] private bool canCastDamage;

    [SerializeField] PlayerHealth playerHealth;

    public void Configure(int damagetAmount, float timeToCastDamage, float maxTimeToStayElectrified)
    {
        this.damage = damagetAmount;
        this.timeToCastDamage = timeToCastDamage;
        canCastDamage = false;
        damageTimer = 0;

        Invoke("RemoveFromScene", maxTimeToStayElectrified);
    }

    private void Update()
    {
        damageTimer += Time.deltaTime;
        
        if(damageTimer >= timeToCastDamage)
        {
            canCastDamage = true;
        }

        if (canCastDamage)
        {
            playerHealth?.TakeDamage(damage);
            damageTimer = 0;
            canCastDamage = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth pH = collision.GetComponent<PlayerHealth>();
        if(pH != null)
        {
            playerHealth = pH; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerHealth = null;
    }

    private void RemoveFromScene()
    {
        CyclingElectricPlatform.ResetTimer();
        Destroy(gameObject);
    }
}
