using System;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public GameObject orbPrefab;
    public Transform playerPosition;
    public HealthBar healthBar;
    private Animator bossAnimator;
    private RandomPitchAudioSource audioSource;

    [Header("Parameters")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Parameters - Orb")]
    public float orbSpeed = 5f;
    public float orbShotIntervalSec = 2f;
    private float shotTimer = 0f;
    public AudioClip orbShotSFX;


    void Start()
    {
        bossAnimator = GetComponent<Animator>();
        audioSource = GetComponent<RandomPitchAudioSource>();

        // Initialize boss health
        currentHealth = maxHealth;
        bossAnimator.SetInteger("bossHealth", currentHealth);
        healthBar.SetMaxHealth(maxHealth);  // Set the max health for the health bar
    }

    void Update()
    {
        shotTimer += Time.deltaTime;

        if (shotTimer >= orbShotIntervalSec)
        {
            ShootOrb();
            shotTimer = 0f;
        }
    }

    private void ShootOrb()
    {
        Vector2 direction = (Vector2)Vector3.Normalize(playerPosition.position - this.transform.position);
        GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
        orb.GetComponent<Rigidbody2D>().velocity = direction * orbSpeed;
        audioSource.PlayAudioWithRandomPitch(orbShotSFX);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        bossAnimator.SetInteger("bossHealth", currentHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}