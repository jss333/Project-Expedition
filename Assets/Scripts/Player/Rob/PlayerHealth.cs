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
    public GameObject defeatScreen; // Reference to your defeat screen
    public GameObject pauseMenu; // Reference to the PauseMenu script
    public static bool IsDefeated = false; // Track if the player is defeated
    [Header("Parameters")]
    public int maxHealth = 150;
    public int currentHealth;
    public AudioClip damageTakenSFX;


    void Start()
    {
        audioSource = GetComponent<RandomPitchAudioSource>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        defeatScreen.SetActive(false); // Make sure the defeat screen is hidden initially
    }

    public void TakeDamage(int healthdamage)
    {
        currentHealth -= healthdamage;
        healthBar.SetHealth(currentHealth);
        audioSource.PlayAudioWithNormalPitch(damageTakenSFX);


        if(currentHealth <= 0)
        {
            ActivateDefeatScreen();
        }
    }
    private void ActivateDefeatScreen()
    {
        IsDefeated = true; // Mark the player as defeated

        // Deactivate the pause menu if it's active
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }

        defeatScreen.SetActive(true); // Show defeat screen immediately
        Time.timeScale = 0f; // Pause the game
        Destroy(robBertParentObj);
        challengeRoomBGM.PlayDefeatBGM();
    }
}

    

