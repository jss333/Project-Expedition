using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    [Header("References")]
    public BossInformation bossInfo;

    [Header("Properties")]
    //Basic properties and component refs...
    public GameObject anchor;
    public MinionSpawnerController spawner;
    public HealthBar healthBar;
    public float maxhealth = 500;
    public float moveSpeed = 3F;
    public float collisionDmg = 5F;
    //private bool awayFromAnchor = true;
    private Rigidbody2D rb;
    public float currentHealth;

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
        currentHealth = maxhealth;
        healthBar.SetHealth((int)(currentHealth / maxhealth * 100f));
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
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage((int)collisionDmg);
        }
        if (collision.gameObject.layer == 8)
        {
            if(reachedAnchor)
            {
                TakeDamage(collision.gameObject.GetComponent<PlayerProjectile>().damageAmt);
                Destroy(collision.gameObject);
            }
        }
        return;
    }
    private IEnumerator moveToAnchor()
    {
        //Describes the movement taken by the minion as it transitions
        //from its spawn point to its anchor point...
        Vector3 direction = Vector3.Normalize(anchor.transform.position - this.transform.position);
        rb.velocity = (Vector2)(direction * moveSpeed);
        while (true)
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth((int)(currentHealth/maxhealth*100f));
        if (currentHealth <= 0)
        {
            DestroyThisMinion();
        }
    }
    private void DestroyThisMinion()
    {
        bossInfo.MinionDestroyed();
        spawner.decrementActiveCount(anchor);
        Destroy(this.gameObject);
    }
}