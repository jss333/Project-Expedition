using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    public int currentHealth;
    public int currentEnergy;
    public int currentArmor;

    public HealthBar healthBar;
    public HealthBar energyShield;
    public HealthBar armorDial;

    void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxHealth;
        currentArmor = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        energyShield.SetMaxHealth(maxHealth);
        armorDial.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int healthdamage)
    {
        currentHealth -= healthdamage;
        //Debug.Log("Dmg taken...");
        healthBar.SetHealth(currentHealth);
    }
}
