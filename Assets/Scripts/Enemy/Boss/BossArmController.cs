using System.Collections;
using UnityEngine;
using FMODUnity;

public class BossArmController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orbSourcePosition;
    [SerializeField] private EventReference bossArmShootSFX;
    private System.Random random;

    [Header("Parameters - Orb")]
    [SerializeField] private GameObject singleOrbPrefab;
    [SerializeField] private float minShotIntervalSec = 1f;
    [SerializeField] private float maxShotIntervalSec = 2f;
    [SerializeField] private float nextShotTime = 0f;
    [SerializeField] private float minSingleOrbSpeed = 4.5f;
    [SerializeField] private float maxSingleOrbSpeed = 5.5f;
    [SerializeField] private Vector2 orbSize = new Vector2(1f, 1f);
    [SerializeField] private int maxOrbsToFire = 10;

    private int orbsFired = 0;

    [Header("Parameters - Animation")]
    [SerializeField] private float animationWaitTime = 3;
    [SerializeField] Animator shootingArmAnimator;

    private float delayTimer = 0f;


    void Start()
    {
        random = new System.Random();
    }

    void Update()
    {

        if (delayTimer < animationWaitTime)
        {

            delayTimer += Time.deltaTime;
            return;
        }

        ShootOrbIfTimeForNextShot();
        OnMaxBulletsFired();

    }

    private void OnMaxBulletsFired()
    {
        if (orbsFired == maxOrbsToFire)
        {
            DestroyArms();
        }
    }

    private void ShootOrbIfTimeForNextShot()
    {
        if (Time.time >= nextShotTime)
        {
            shootingArmAnimator.Play("Shoot");
            GameObject targetSingleOrb;
            targetSingleOrb = singleOrbPrefab;
            ShootSingleOrb(GetRandomFloat(minSingleOrbSpeed, maxSingleOrbSpeed), targetSingleOrb);

            AudioManagerNoMixers.Singleton.PlayOneShot(bossArmShootSFX, this.transform.position);

            nextShotTime = Time.time + GetRandomFloat(minShotIntervalSec, maxShotIntervalSec);
        }
    }

    private void ShootSingleOrb(float speed, GameObject orbPrefab)
    {
        orbsFired++;
        Vector2 shootDirection = transform.up;
        GameObject orb = Instantiate(orbPrefab, orbSourcePosition.position, Quaternion.identity);
        orb.transform.localScale = new Vector3(orbSize.x, orbSize.y, 1f);
        orb.GetComponent<Rigidbody2D>().velocity = shootDirection.normalized * speed;
    }

    private float GetRandomFloat(float min, float max)
    {
        return (float)(min + (random.NextDouble() * (max - min)));
    }

    private void DestroyArms()
    {
        shootingArmAnimator.Play("Close");
        StartCoroutine(DestroyAfterAnimation(animationWaitTime));
    }

    private IEnumerator DestroyAfterAnimation(float animationWaitTime)
    {
        yield return new WaitForSeconds(animationWaitTime); 
        Destroy(gameObject); 
    }

}