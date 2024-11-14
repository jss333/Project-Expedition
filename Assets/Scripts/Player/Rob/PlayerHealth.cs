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
    private EntityActionVisualController entityActionVisualController;
    private Animator animator;

    [Header("Parameters")]
    public int maxHealth = 150;
    public int currentHealth;

    void Start()
    {
        healthBar = GameObject.Find("Health-border").GetComponent<HealthBar>();
        challengeRoomBGM = FindObjectOfType<ChallengeRoomBGM>();
        entityActionVisualController = GetComponent<EntityActionVisualController>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int healthdamage)
    {
        currentHealth -= healthdamage;
        healthBar.SetHealth(currentHealth);
        AudioManagerNoMixers.Singleton.PlaySFXByName("PlayerTakesDamage");
        entityActionVisualController.ApplyGettingHitVisuals();
        DamageEventsManager.OnPlayerDamaged?.Invoke((float)healthdamage / maxHealth);
        PlayHitAnim();
        if (currentHealth <= 0)
        {
            Destroy(robBertParentObj);
            //challengeRoomBGM.PlayDefeatBGM();
            AudioManagerNoMixers.Singleton.PlayDefeatMusic();
            EndGameEventManager.OnDefeatAchieved?.Invoke();
        }
    }
    private void PlayHitAnim()
    {
        animator.SetTrigger("gotHit");
    }
}
