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
    private ChallengeRoomBGM challengeRoomBGM;
    private Animator bossAnimator;
    private System.Random random;
    private BossInformation bossInfo;
    private EntityActionVisualController bossAnimationController;
    private CircleCollider2D circleCollider;
    private bool bossDeath = false;

    [Header("Parameters")]
    [SerializeField] private float hurtStateHealthPercent = 50f;
    public bool hurtStateTriggered = false;
    [SerializeField] private float bgmChangeHealthPercent = 40f;
    private bool bgmChangeTriggered = false;
    [SerializeField] private float damageTakenSFXCooldown = 0.2f;
    private float lastDamageTakenSFXPlayTime = -Mathf.Infinity;

    [SerializeField] private float delayEndScreen = 10f;

    [Header("Parameters - Minion/shield respawn")]
    [SerializeField] private List<float> minionRespawnThreasholds;

    [Header("Parameters - Orb")]
    [SerializeField] private GameObject singleOrbPrefab;
    [SerializeField] private float minShotIntervalSec = 1f;
    [SerializeField] private float maxShotIntervalSec = 2f;
    [SerializeField] private float nextShotTime = 0f;
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
    private BossHealthComponent bossHealthComponent;
    private int trackOldHealth;
    void Start()
    {
        random = new System.Random();
        playerPosition = FindFirstObjectByType<PlayerMovement>().gameObject.transform;
        challengeRoomBGM = FindFirstObjectByType<ChallengeRoomBGM>();
        bossAnimator = GetComponent<Animator>();
        bossInfo = GetComponent<BossInformation>();
        bossAnimationController = GetComponent<EntityActionVisualController>();
        circleCollider = GetComponent<CircleCollider2D>();
        bossHealthComponent = GetComponent<BossHealthComponent>();

        trackOldHealth = bossHealthComponent.getCurrentHealth();

        nextShotTime = Time.time + 3f;

        if(minionRespawnThreasholds.Count == 0 && trackOldHealth == bossHealthComponent.getMaxHealth())
        {
            instantiateBossShield();
        }
    }

    void Update()
    {
        if (PlayerIsAlive() && !bossDeath)
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
                ShootMultipleOrbs(GetDirectionToPlayer(), GetRandomizedSpeed(minMultipleOrbSpeed, maxMultipleOrbSpeed));
                AudioManagerNoMixers.Singleton.PlaySFXByName("BossShootsMultipleProjectiles");
            }
            else
            {
                ShootSingleOrb(GetDirectionToPlayer(), GetRandomizedSpeed(minSingleOrbSpeed, maxSingleOrbSpeed), singleOrbPrefab);
                AudioManagerNoMixers.Singleton.PlaySFXByName("BossShootsSingleProjectile");
            }

            nextShotTime = Time.time + GetRandomFloat(minShotIntervalSec, maxShotIntervalSec);
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

    public void BossHit()
    {
        //spawn popup label
        if(trackOldHealth != bossHealthComponent.getCurrentHealth())
        {
            bossHealthComponent.SpawnDamageNumberPopupLabel(trackOldHealth - bossHealthComponent.getCurrentHealth());
            trackOldHealth = bossHealthComponent.getCurrentHealth();
        }
        minionRespawn();
        //healthBar.SetHealth(currentHealth);
        PlayDamageTakenSFXIfEnoughCooldownTimeHasPassed();
        bossAnimationController.ApplyGettingHitVisuals();
        CheckHurtState();
    }

    private void PlayDamageTakenSFXIfEnoughCooldownTimeHasPassed()
    {
        if (Time.time >= lastDamageTakenSFXPlayTime + damageTakenSFXCooldown)
        {
            AudioManagerNoMixers.Singleton.PlaySFXByName("BossTakesDamage");
            lastDamageTakenSFXPlayTime = Time.time;
        }
    }

    public void Die()
    {
        AudioManagerNoMixers.Singleton.PlayVictroyMusic();
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
                if (bossHealthComponent.CurrentHealthPercentLessThan(minionRespawnThreasholds[i]))
                {

                    instantiateBossShield();
                    minionRespawnThreasholds.RemoveAt(i);
                    FindAnyObjectByType<MinionSpawnerController>().handleMinionRespawn();
                    AudioManagerNoMixers.Singleton.PlaySFXByName("BossSpawnsMinions");
                    bossHealthComponent.SetIsImmune(true);
                }
            }
        }
    }

    public void setHasShield(bool value)
    {
        hasShield = value;
    }
    private void CheckHurtState()
    {
        if (!hurtStateTriggered && bossHealthComponent.CurrentHealthPercentLessThan(hurtStateHealthPercent))
        {
            hurtStateTriggered = true; //Used to control which shooting animation to play by EntityActionVisualController
        }

        if (!bgmChangeTriggered && bossHealthComponent.CurrentHealthPercentLessThan(bgmChangeHealthPercent))
        {
            //challengeRoomBGM.PlaySecondHalfBGM();
            AudioManagerNoMixers.Singleton.PlaySecondPartMusic();
            bgmChangeTriggered = true;
        }
    }
}