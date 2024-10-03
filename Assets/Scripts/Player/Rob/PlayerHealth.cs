using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    public HealthBar healthBar;
    public GameObject robBertParentObj;
    public ChallengeRoomBGM challengeRoomBGM;
    private RandomPitchAudioSource audioSource;

    [Header("Parameters")]
    public int maxHealth = 150;
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

        if(currentHealth <= 0)
        {
            Destroy(robBertParentObj);
            challengeRoomBGM.PlayDefeatBGM();
            EndGameEventManager.OnDefeatAchieved?.Invoke();

        }
    }
}
