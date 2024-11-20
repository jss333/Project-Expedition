using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    [Header("References")]
    private BossInformation bossInfo;

    [Header("Properties")]
    //Basic properties and component refs...
    public GameObject anchor;
    private MinionSpawnerController spawner;
    public float moveSpeed = 3F;
    public float collisionDmg = 5F;
    //private bool awayFromAnchor = true;
    private Rigidbody2D rb;

    [Header("Parameters")]
    //Parameters describing the minion's attack properties...
    [SerializeField] private GameObject projectile;
    public float attackPeriod = 2F;
    [SerializeField] private float attackTimer = 0;
    public int burstSize = 3;
    public float burstDensity = .2F;
    [SerializeField] private float burstTimer = 0;
    [SerializeField] private int shotNum = 0;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private Quaternion launchAngle;

    private bool reachedAnchor;
    [Header("Audio")]
    private bool minionHitCooldown = false;
    private float finishHitCooldown = 0;
    public void Start()
    {
        reachedAnchor = false;
        spawner = FindAnyObjectByType<MinionSpawnerController>();
        bossInfo = FindAnyObjectByType<BossInformation>();
        rb = GetComponent<Rigidbody2D>();
        if (projectile == null || launchPoint == null)
        {
            Debug.Log("Minion Controller: projectile spawn parameters null -- component disabled...");
            this.gameObject.SetActive(false);
            return;
        }
        if (this.transform.position != anchor.transform.position)
        {
            StartCoroutine(moveToAnchor());
        }
        burstTimer = burstDensity;
        return;
    }

    public void Update()
    {
        if (attackTimer >= attackPeriod && reachedAnchor)
        {
            attackTimer = 0;
            StartCoroutine(Attack());
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }

    public IEnumerator Attack()
    {
        for (shotNum = 0; shotNum < burstSize;)
        {
            if (burstTimer >= burstDensity)
            {
                Instantiate(projectile, this.transform.position, this.transform.rotation);
                burstTimer = 0;
                shotNum++;
            }
            else { burstTimer += Time.deltaTime; }
            yield return null;
        }

        burstTimer = burstDensity;
        attackTimer = 0;
        yield break;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //dealing damage to the player on impact.
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealthComponent>().TakeDamage((int)collisionDmg);
        }
        return;
    }
    private IEnumerator moveToAnchor()
    {
        //Describes the movement taken by the minion as it transitions
        //from its spawn point to its anchor point...
        Vector3 direction = Vector3.Normalize(anchor.transform.position - this.transform.position);
        rb.velocity = (Vector2)(direction * moveSpeed);
        while (!reachedAnchor)
        {
            if ((this.transform.position - anchor.transform.position).magnitude <= .2F)
            {
                this.transform.position = anchor.transform.position;
                reachedAnchor = true;
                rb.velocity = Vector2.zero;
                yield break;
            }
            yield return null;
        }
    }
    //add this the the death unity event on the health comp
    public void DestroyThisMinion()
    {
        bossInfo.MinionDestroyed();
        //allow the taken anchor to be available again
        spawner.decrementActiveCount(anchor);
        //play death sound
        AudioManagerNoMixers.Singleton.PlaySFXByName("MinionDeath");
        Destroy(this.gameObject);
    }
    public void MinionHit()
    {
        //the minion is immune while moving to the anchor
        if (reachedAnchor)
        {
            //if the sfx is not on cooldown
            if (!minionHitCooldown)
            {
                finishHitCooldown = Time.time + 1f;
                minionHitCooldown = true;
                AudioManagerNoMixers.Singleton.PlaySFXByName("MinionHit");
            }
            //the sfx cooldown is finished
            if (finishHitCooldown < Time.time)
            {
                minionHitCooldown = false;
            }
        }
    }
}