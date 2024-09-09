using System;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform orbSourcePosition;
    public Transform playerPosition;
    public HealthBar healthBar;
    private Animator bossAnimator;
    private RandomPitchAudioSource audioSource;
    private System.Random random;

    [Header("Parameters")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Parameters - Orb")]
    public GameObject singleOrbPrefab;
    public float minOrbSpeed = 4.5f;
    public float maxOrbSpeed = 5.5f;
    public float minShotIntervalSec = 0.7f;
    public float maxShotIntervalSec = 1.3f;
    private float nextShotTime = 0f;
    public AudioClip orbShotSFX;

    [Header("Parameters - Multiple orbs")]
    public GameObject multipleOrbPrefab;
    public float multipleOrbsPercent = 10f;
    public float numMultipleOrbs = 5;
    public float multipleOrbsAngleSpreadDeg = 60;
    public AudioClip multipleOrbShotSFX;


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
        ShootOrbIfTimeForNextShot();
    }

    private void ShootOrbIfTimeForNextShot()
    {
        if (Time.time >= nextShotTime)
        {
            if (WillShootMultipleOrbs() && numMultipleOrbs > 1)
            {
                ShootMultipleOrbs(GetDirectionToPlayer(), GetRandomizedSpeed());
                audioSource.PlayAudioWithRandomPitch(multipleOrbShotSFX);
            }
            else
            {
                ShootSingleOrb(GetDirectionToPlayer(), GetRandomizedSpeed(), singleOrbPrefab);
                audioSource.PlayAudioWithRandomPitch(orbShotSFX);
            }

            nextShotTime += GetRandomFloat(minShotIntervalSec, maxShotIntervalSec);
        }
    }

    private bool WillShootMultipleOrbs()
    {
        return GetRandomFloat(0, 99) < multipleOrbsPercent;
    }

    // Shoots numMultipleOrbs shots spread out evenly in a cone of multipleOrbsAngleSpreadDeg angles centered on the player
    private void ShootMultipleOrbs(Vector2 directionToPlayer, float speed)
    {
        float angleBetweenOrbs = multipleOrbsAngleSpreadDeg / (numMultipleOrbs - 1);
        float angleToMostCounterclockwiseDirection = multipleOrbsAngleSpreadDeg / 2;
        Vector2 mostCounterclokwiseDirection = Rotate(directionToPlayer, angleToMostCounterclockwiseDirection, false);

        Vector2 nextDirection = mostCounterclokwiseDirection;
        for (int i = 0; i < numMultipleOrbs; i++)
        {
            ShootSingleOrb(nextDirection, speed, multipleOrbPrefab);
            nextDirection = Rotate(nextDirection, angleBetweenOrbs, true);
        }
    }

    private void ShootSingleOrb(Vector2 directionToPlayer, float speed, GameObject orbPrefab)
    {
        GameObject orb = Instantiate(orbPrefab, orbSourcePosition.position, Quaternion.identity);
        orb.GetComponent<Rigidbody2D>().velocity = directionToPlayer * speed;
    }

    private Vector2 GetDirectionToPlayer()
    {
        return (Vector2)Vector3.Normalize(playerPosition.position - orbSourcePosition.position);
    }

    private float GetRandomizedSpeed()
    {
        return GetRandomFloat(minOrbSpeed, maxOrbSpeed);
    }

    public Vector2 Rotate(Vector2 dir, float degrees, bool clockwise)
    {
        // Convert the degrees to radians
        float radians = degrees * Mathf.Deg2Rad;

        // Calculate the sine and cosine of the angle
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        // If rotating clockwise, invert the sin component
        if (clockwise)
        {
            sin = -sin;
        }

        // Apply the 2D rotation matrix
        float newX = dir.x * cos - dir.y * sin;
        float newY = dir.x * sin + dir.y * cos;

        // Return the new rotated Vector2
        return new Vector2(newX, newY);
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