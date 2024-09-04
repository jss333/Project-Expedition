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
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxHealth;
        currentArmor = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        energyShield.SetMaxHealth(maxHealth);
        armorDial.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {        
    }

    public void TakehealthDamage(int healthdamage)
    {
        currentHealth -= healthdamage;
        healthBar.SetHealth(currentHealth);

    }

    public void TakeEnergyDamage(int energydamage)
    {
        currentEnergy -= energydamage;
        energyShield.SetHealth(currentEnergy);
    }

    public void TakeArmorDamage(int armorDamage)
    {
        currentArmor -= armorDamage;
        armorDial.SetHealth(currentArmor);
    }
}
