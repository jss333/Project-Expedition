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
    private System.Random random;

    [Header("Parameters")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Parameters - Orb")]
    public float minOrbSpeed = 4.5f;
    public float maxOrbSpeed = 5.5f;
    public float minShotIntervalSec = 0.7f;
    public float maxShotIntervalSec = 1.3f;
    private float nextShotTime = 0f;
    public AudioClip orbShotSFX;


    void Start()
    {
        bossAnimator = GetComponent<Animator>();
        audioSource = GetComponent<RandomPitchAudioSource>();
        random = new System.Random();

        currentHealth = maxHealth;
        bossAnimator.SetInteger("bossHealth", currentHealth);
        healthBar.SetMaxHealth(maxHealth);

        nextShotTime = Time.time + 3f;
    }

    void Update()
    {
        if (Time.time >= nextShotTime)
        {
            ShootOrb();
            nextShotTime += GetRandomFloat(minShotIntervalSec, maxShotIntervalSec);
        }
    }

    private void ShootOrb()
    {
        Vector2 direction = (Vector2)Vector3.Normalize(playerPosition.position - this.transform.position);
        GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
        orb.GetComponent<Rigidbody2D>().velocity = direction * GetRandomFloat(minOrbSpeed, maxOrbSpeed);
        audioSource.PlayAudioWithRandomPitch(orbShotSFX);
    }

    private float GetRandomFloat(float min, float max)
    {
        return (float)(min + (random.NextDouble() * (max - min)));
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