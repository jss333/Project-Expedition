using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private Sprite deathSprite;
    [SerializeField] private Transform orbSourcePosition;
    [SerializeField] private GameObject p_BossShield;
    [SerializeField] private Transform p_BossShieldSpawnPoint;
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
    [SerializeField] private EventReference bossSpawnMinionSoundEvent;
    [SerializeField] private List<float> minionRespawnThreasholds;

    [Header("Parameters - Orb")]
    [SerializeField] private GameObject singleOrbPrefab;
    [SerializeField] private GameObject singleStunOrbPrefab;
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
    private EnemyShootingController enemyShootingController;

    [Header("Parameters - Arms")]
    [SerializeField] private bool bossArmAttackEnabled;
    [SerializeField] private GameObject drillArmPrefab;
    [SerializeField] private Animator drillArmAnimator;
    [SerializeField] private GameObject shootingArmPrefab;
    [SerializeField] private Transform spawnPoints = null;
    [SerializeField] private float initialWaitTime = 15;
    [SerializeField] private float loopWaitTime = 15;
    [SerializeField] private EventReference bossArmStartSirenSFX;
    [SerializeField] private EventReference bossArmDeploySFX;


    private List<Transform> verticalSpawnPoints = new List<Transform>();
    private List<Transform> horizontalSpawnPoints = new List<Transform>();
    private GameObject leftArm = null;
    private GameObject rightArm = null;

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
        enemyShootingController = GetComponent<EnemyShootingController>();


        trackOldHealth = bossHealthComponent.getCurrentHealth();

        nextShotTime = Time.time + 3f;

        if (minionRespawnThreasholds.Count == 0 && trackOldHealth == bossHealthComponent.getMaxHealth())
        {
            instantiateBossShield();
        }

        if (bossArmAttackEnabled)
        {
            StartCoroutine(StartArmLoop());
        }
    }

    void Update()
    {
        if (PlayerIsAlive() && !bossDeath)
        {
            ShootOrbIfTimeForNextShot();
        }
    }

    private IEnumerator StartArmLoop()
    {
        yield return new WaitForSeconds(initialWaitTime);

        ObtainShootingArmSpawnPoints();

        AudioManagerNoMixers.Singleton.PlayOneShot(bossArmStartSirenSFX, this.transform.position);
        yield return new WaitForSeconds(1.0f);

        drillArmAnimator.Play("DrillStart");

        AudioManagerNoMixers.Singleton.PlayOneShot(bossArmDeploySFX, this.transform.position);

        yield return new WaitForSeconds(2.02f);

        drillArmAnimator.Play("DrillLoop");

        SpawnShootingArms();
        StartCoroutine(ArmLoop());
    }

    private IEnumerator ArmLoop()
    {
        while (true)
        {
            if (rightArm == null && leftArm == null)
            {
                drillArmAnimator.Play("DrillEnd");
                AudioManagerNoMixers.Singleton.PlayOneShot(bossArmDeploySFX, this.transform.position);

                yield return new WaitForSeconds(2.02f);
                drillArmAnimator.Play("DrillIdle");

                yield return new WaitForSeconds(loopWaitTime);

                AudioManagerNoMixers.Singleton.PlayOneShot(bossArmStartSirenSFX, this.transform.position);
                yield return new WaitForSeconds(1.0f);

                drillArmAnimator.Play("DrillStart");
                AudioManagerNoMixers.Singleton.PlayOneShot(bossArmDeploySFX, this.transform.position);

                yield return new WaitForSeconds(2.02f);

                drillArmAnimator.Play("DrillLoop");

                ObtainShootingArmSpawnPoints();
                SpawnShootingArms();
 
            }
            yield return null;
        }
    }

    public void SpawnShootingArms()
    {
        if ((verticalSpawnPoints.Count + horizontalSpawnPoints.Count) < 2)
        {
            Debug.LogWarning("At least 2 spawn points are required in the arm spawn points list.");
            return;
        }

        Transform firstSpawnPoint = GetRandomSpawnPoint(BossArmSpawnPoint.SpawnType.Vertical);
        Transform secondSpawnPoint = GetRandomSpawnPoint(BossArmSpawnPoint.SpawnType.Horizontal);

        leftArm = Instantiate(shootingArmPrefab, firstSpawnPoint.position, firstSpawnPoint.rotation);
        rightArm = Instantiate(shootingArmPrefab, secondSpawnPoint.position, secondSpawnPoint.rotation);

    }
    private void ObtainShootingArmSpawnPoints()
    {
        verticalSpawnPoints.Clear();
        horizontalSpawnPoints.Clear();

        foreach (Transform spawnPointChild in spawnPoints)
        {
            BossArmSpawnPoint spawnPoint = spawnPointChild.GetComponent<BossArmSpawnPoint>();
            if (spawnPoint != null)
            {
                if (spawnPoint.spawnType == BossArmSpawnPoint.SpawnType.Vertical)
                    verticalSpawnPoints.Add(spawnPointChild);
                else if (spawnPoint.spawnType == BossArmSpawnPoint.SpawnType.Horizontal)
                    horizontalSpawnPoints.Add(spawnPointChild);
            }
        }
    }

    private Transform GetRandomSpawnPoint(BossArmSpawnPoint.SpawnType spawnType)
    {

        if (spawnType == BossArmSpawnPoint.SpawnType.Vertical)
        {
            int randomIndex = random.Next(0, verticalSpawnPoints.Count);
            return verticalSpawnPoints[randomIndex];
        } 
        else if (spawnType == BossArmSpawnPoint.SpawnType.Horizontal)
        {
            int randomIndex = random.Next(0, horizontalSpawnPoints.Count);
            return horizontalSpawnPoints[randomIndex];
        } 
        else
        {
            return null;
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
                GameObject targetSingleOrb;
                if (enemyShootingController.CanShootStunBullet())
                {
                    targetSingleOrb = singleStunOrbPrefab;
                }
                else
                {
                    targetSingleOrb = singleOrbPrefab;
                }
                ShootSingleOrb(GetDirectionToPlayer(), GetRandomizedSpeed(minSingleOrbSpeed, maxSingleOrbSpeed), targetSingleOrb);
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
        if (trackOldHealth != bossHealthComponent.getCurrentHealth())
        {
            bossHealthComponent.SpawnDamageNumberPopupLabel(bossHealthComponent.getMaxHealth() - bossHealthComponent.getCurrentHealth());
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
        GameObject shieldInstacne = Instantiate(p_BossShield, p_BossShieldSpawnPoint.position, p_BossShieldSpawnPoint.rotation, p_BossShieldSpawnPoint);
        shieldInstacne.transform.localPosition = Vector3.zero;
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
                    FindAnyObjectByType<MinionSpawnerController>().HandleMinionRespawn();
                    AudioManagerNoMixers.Singleton.PlayOneShot(bossSpawnMinionSoundEvent, this.transform.position);
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