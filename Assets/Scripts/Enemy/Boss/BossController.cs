using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform orbSourcePosition;
    public Transform playerPosition;
    public HealthBar healthBar;
    public ChallengeRoomBGM challengeRoomBGM;
    private Animator bossAnimator;
    private RandomPitchAudioSource audioSource;
    private System.Random random;
    private BossInformation info;
    [SerializeField] private GameObject p_BossShield;
    [SerializeField] private EntityActionVisualController bossAnimationController;

    [Header("Parameters")]
    public int maxHealth = 5000;
    public int currentHealth;
    public float hurtStateHealthPercent = 50f;
    private bool hurtStateTriggered = false;
    public float bgmChangeHealthPercent = 40f;
    private bool bgmChangeTriggered = false;
    public AudioClip damageTakenSFX;
    public float damageTakenSFXCooldown = 0.2f;
    private float lastDamageTakenSFXPlayTime = -Mathf.Infinity;

    [Header("Parameters - Minion/shield respawn")]
    [SerializeField] private List<float> minionRespawnThreasholds;
    public AudioClip minionRespawnSFX;

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

    private bool hasShield = false;
    void Start()
    {
        bossAnimator = GetComponent<Animator>();
        audioSource = GetComponent<RandomPitchAudioSource>();
        random = new System.Random();
        info = GetComponent<BossInformation>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        nextShotTime = Time.time + 3f;
        instantiateBossShield(); 
    }

    void Update()
    {
        if (PlayerIsAlive())
        {
            ShootOrbIfTimeForNextShot();
        }
    }

    private bool PlayerIsAlive()
    {
        return playerPosition != null;
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
        bossAnimationController.ApplyShootAnimation();
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
        if(info.getImmune())
        {
            return;
        }
        else
        {

            currentHealth -= damage;
            minionRespawn();
            healthBar.SetHealth(currentHealth);
            PlayDamageTakenSFXIfEnoughCooldownTimeHasPassed();
            bossAnimationController.ApplyGettingHitVisuals();

            if (!hurtStateTriggered && CurrentHealthPercentLessThan(hurtStateHealthPercent))
            {
                bossAnimator.SetTrigger("bossHurt");
                hurtStateTriggered = true;
            }

            if (!bgmChangeTriggered && CurrentHealthPercentLessThan(bgmChangeHealthPercent))
            {
                challengeRoomBGM.PlaySecondHalfBGM();
                bgmChangeTriggered = true;
            }

            if (currentHealth <= 0)
            {
                challengeRoomBGM.PlayVictoryBGM();
                Die();
            }
        }
    }

    private void PlayDamageTakenSFXIfEnoughCooldownTimeHasPassed()
    {
        if (Time.time >= lastDamageTakenSFXPlayTime + damageTakenSFXCooldown)
        {
            audioSource.PlayAudioWithRandomPitch(damageTakenSFX);
            lastDamageTakenSFXPlayTime = Time.time;
        }
    }

    private bool CurrentHealthPercentLessThan(float thresholdPercent)
    {
        return (float)currentHealth / maxHealth <= thresholdPercent / 100;
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            TakeDamage(collision.gameObject.GetComponent<PlayerProjectile>().damageAmt);
            Destroy(collision.gameObject);
            
        }
    }
    private void instantiateBossShield()
    {
        Instantiate(p_BossShield, this.transform.position, Quaternion.identity);
        hasShield = true;
        Debug.Log("BossShield Up");
    }
    private void minionRespawn()
    {
        if (!hasShield)
        {
            for (int i = 0; i < minionRespawnThreasholds.Count(); i++)
            {
                if (CurrentHealthPercentLessThan(minionRespawnThreasholds[i]))
                {
                    instantiateBossShield();
                    minionRespawnThreasholds.RemoveAt(i);
                    FindAnyObjectByType<MinionSpawnerController>().handleMinionRespawn();
                    audioSource.PlayAudioWithRandomPitch(minionRespawnSFX);
                    info.setImmune(true);
                }
            }
        }
    }
    public void setHasShield(bool value)
    {
        hasShield = value;
    }
}