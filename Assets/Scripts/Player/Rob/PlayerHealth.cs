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
        playHitAnim();
        if (currentHealth <= 0)
        {
            Destroy(robBertParentObj);
            challengeRoomBGM.PlayDefeatBGM();
            EndGameEventManager.OnDefeatAchieved?.Invoke();
        }
    }
    private void playHitAnim()
    {
        animator.SetBool("isHit", true);
        //transform.localScale = new Vector3(.4f, .4f, .4f);
    }
    public void stopHitAnim()
    {
        animator.SetBool("isHit", false);
        //transform.localScale = Vector3.one;
    }
}
