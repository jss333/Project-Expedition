using System;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public GameObject orbPrefab;  // Assign the orb prefab in the inspector
    public Transform player;      // Assign the player's transform in the inspector
    public Sprite beatenUpSprite; // Assign the beaten up sprite in the inspector
    public float orbSpeed = 5f;   // Speed of the orb
    public float shootInterval = 2f; // Interval between shots
    private float shootTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Animator bossAnimator;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;  // Link to the health bar UI element

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossAnimator = GetComponent<Animator>();

        // Initialize boss health
        currentHealth = maxHealth;
        bossAnimator.SetInteger("bossHealth", currentHealth);
        healthBar.SetMaxHealth(maxHealth);  // Set the max health for the health bar
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootOrb();
            shootTimer = 0f;
        }
    }

    void ShootOrb()
    {
        Vector2 direction = (Vector2)Vector3.Normalize(player.position - this.transform.position);
        GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
        orb.GetComponent<Rigidbody2D>().velocity = direction * orbSpeed;
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

    void Die()
    {
        Destroy(this.gameObject);
    }
}