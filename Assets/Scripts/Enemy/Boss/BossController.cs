using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private Sprite deathSprite;
    [SerializeField] private Transform orbSourcePosition;
    [SerializeField] private GameObject p_BossShield;
    private Transform playerPosition;
    private HealthBar healthBar;
    private ChallengeRoomBGM challengeRoomBGM;
    private Animator bossAnimator;
    private System.Random random;
    private BossInformation info;
    private EntityActionVisualController bossAnimationController;
    private CircleCollider2D circleCollider;
    private bool bossDeath = false;

    [Header("References - Popup labels")]
    [SerializeField] private PopupLabel damageNumberPopupPrefab;
    [SerializeField] private PopupLabel immunePopupPrefab;
    [SerializeField] private Transform popupLabelSource;

    [Header("Parameters")]
    [SerializeField] private int maxHealth = 5000;
    private int currentHealth;
    [SerializeField] private float hurtStateHealthPercent = 50f;
    public bool hurtStateTriggered = false;
    [SerializeField] private float bgmChangeHealthPercent = 40f;
    private bool bgmChangeTriggered = false;
    [SerializeField] private float damageTakenSFXCooldown = 0.2f;
    private float lastDamageTakenSFXPlayTime = -Mathf.Infinity;
    [SerializeField] private float stopOverflowDamageNumbers = 1f;
    [SerializeField] private float overflowDamageCooldown = 1f;
    [SerializeField] private float delayEndScreen = 10f;

    [Header("Parameters - Minion/shield respawn")]
    [SerializeField] private List<float> minionRespawnThreasholds;

    [Header("Parameters - Orb")]
    [SerializeField] private GameObject singleOrbPrefab;
    [SerializeField] private float minShotIntervalSec = 0.7f;
    [SerializeField] private float maxShotIntervalSec = 1.3f;
    private float nextShotTime = 0f;
    [SerializeField] private float minSingleOrbSpeed = 4.5f;
    [SerializeField] private float maxSingleOrbSpeed = 5.5f;

    [Header("Parameters - Multiple orbs")]
    [SerializeField] private GameObject multipleOrbPrefab;
    [SerializeField] private float multipleOrbsPercent = 10f;
    [SerializeField] private float numMultipleOrbs = 5;
    [SerializeField] private float multipleOrbsAngleSpreadDeg = 60;
    [SerializeField] private float minMultipleOrbSpeed = 4.5f;
    [SerializeField] private float maxMultipleOrbSpeed = 5.5f;

    private bool hasShield = false;
    private bool damageNumActive = false;

    void Start()
    {
        random = new System.Random();
        playerPosition = FindFirstObjectByType<PlayerMovement>().gameObject.transform;
        challengeRoomBGM = FindFirstObjectByType<ChallengeRoomBGM>();
        healthBar = GetComponentInChildren<HealthBar>();
        bossAnimator = GetComponent<Animator>();
        info = GetComponent<BossInformation>();
        bossAnimationController = GetComponent<EntityActionVisualController>();
        circleCollider = GetComponent<CircleCollider2D>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        nextShotTime = Time.time + 3f;
        instantiateBossShield();

        stopOverflowDamageNumbers = overflowDamageCooldown;
        bossAnimator.SetBool("IdleState", true);
    }

    void Update()
    {
        if (PlayerIsAlive() && !bossDeath)
        {
            ShootOrbIfTimeForNextShot();
        }
        if (damageNumActive)
        {
            stopOverflowDamageNumbers -= Time.deltaTime;
            if(stopOverflowDamageNumbers <= -3)
            {
                damageNumActive = false;
            }
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
                ShootMultipleOrbs(GetDirectionToPlayer(), GetRandomizedSpeed(minMultipleOrbSpeed, maxMultipleOrbSpeed));
                AudioManagerNoMixers.Singleton.PlaySFXByName("BossShootsMultipleProjectiles");
            }
            else
            {
                ShootSingleOrb(GetDirectionToPlayer(), GetRandomizedSpeed(minSingleOrbSpeed, maxSingleOrbSpeed), singleOrbPrefab);
                AudioManagerNoMixers.Singleton.PlaySFXByName("BossShootsSingleProjectile");
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

    private float GetRandomizedSpeed(float minSpeed, float maxSpeed)
    {
        return GetRandomFloat(minSpeed, maxSpeed);
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
        if(info.GetImmune())
        {
            if (damageNumActive == false)
            {
                damageNumActive = true;
                SpawnImmunePopupLabel();
            }
            else if(damageNumActive && stopOverflowDamageNumbers < 0)
            {
                SpawnImmunePopupLabel();
            }
            return;
        }
        else
        {
            SpawnDamageNumberPopupLabel(damage);

            currentHealth -= damage;
            minionRespawn();
            healthBar.SetHealth(currentHealth);
            PlayDamageTakenSFXIfEnoughCooldownTimeHasPassed();
            bossAnimationController.ApplyGettingHitVisuals();

            if (!hurtStateTriggered && CurrentHealthPercentLessThan(hurtStateHealthPercent))
            {
                bossAnimator.SetTrigger("bossHurt");
                hurtStateTriggered = true;
                bossAnimator.SetBool("IdleStateDamaged", true);
                bossAnimator.SetBool("IdleState", false);
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
            AudioManagerNoMixers.Singleton.PlaySFXByName("BossTakesDamage");
            lastDamageTakenSFXPlayTime = Time.time;
        }
    }

    private bool CurrentHealthPercentLessThan(float thresholdPercent)
    {
        return (float)currentHealth / maxHealth <= thresholdPercent / 100;
    }

    private void Die()
    {
        bossDeath = true;
        circleCollider.radius = 0;
        //play death animation
        bossAnimator.SetTrigger("bossDeath");
        //transform.position += new Vector3(0f, .2f, 0f);
        Invoke("EndGame", delayEndScreen);
        
    }
    public void EndGame()
    {
        EndGameEventManager.OnVictoryAchieved?.Invoke();
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
                    AudioManagerNoMixers.Singleton.PlaySFXByName("BossSpawnsMinions");
                    info.SetImmune(true);
                }
            }
        }
    }

    public void setHasShield(bool value)
    {
        hasShield = value;
    }

    private void SpawnImmunePopupLabel()
    {
        Instantiate(immunePopupPrefab, popupLabelSource.position, Quaternion.identity);
        stopOverflowDamageNumbers = overflowDamageCooldown;
    }
    private void SpawnDamageNumberPopupLabel(int damage)
    {
        //quick hits will stack numbers (future)
        PopupLabel dmgNumPopup = Instantiate(damageNumberPopupPrefab, popupLabelSource.position, Quaternion.identity);
        dmgNumPopup.UpdateLabel(damage.ToString());
    }
}