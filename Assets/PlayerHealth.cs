using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    public HealthBar healthBar;
    private RandomPitchAudioSource audioSource;

    [Header("Parameters")]
    public int maxHealth = 200;
    public int currentHealth;
    public AudioClip damageTakenSFX;


    void Start()
    {
        audioSource = GetComponent<RandomPitchAudioSource>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int healthdamage)
    {
        currentHealth -= healthdamage;
        healthBar.SetHealth(currentHealth);
        audioSource.PlayAudioWithNormalPitch(damageTakenSFX);
    }
}
