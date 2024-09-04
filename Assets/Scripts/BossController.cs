//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class BossController : MonoBehaviour
//{
//    public GameObject orbPrefab;  // Assign the orb prefab in the inspector
//    public Transform player;      // Assign the player's transform in the inspector
//    public Sprite beatenUpSprite; // Assign the beaten up sprite in the inspector
//    public float orbSpeed = 5f;   // Speed of the orb
//    public float shootInterval = 2f; // Interval between shots
//    private float shootTimer = 0f;
//    private SpriteRenderer spriteRenderer;
//    public int bossHealth = 100;
//    private Animator bossAnimator;
//    public HealthBar healthBar;

//    public int bossCurrentHealth;


//    public Slider bossHealthBar;
//    void Start()
//    {
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        bossAnimator = GetComponent<Animator>();
//        bossCurrentHealth = bossHealth;
//        healthBar.SetMaxHealth(bossHealth);
//    }

//    void Update()
//    {
//        shootTimer += Time.deltaTime;

//        if (shootTimer >= shootInterval)
//        {
//            ShootOrb();
//            shootTimer = 0f;
//            bossHealthBar.value = bossCurrentHealth;
//        }

//        //bossAnimator.SetInteger("bossHealth", bossCurrentHealth);
//        //TakeDamage(2);
//    }

//    public void TakeDamage(int damage)
//    {

//        bossCurrentHealth = System.Math.Max(0, bossCurrentHealth - damage);
//        healthBar.SetHealth(bossCurrentHealth);


//        bossAnimator.SetInteger("bossHealth", bossCurrentHealth);

//    }

//    void ShootOrb()
//    {
//        // Vector2 direction = player.position.x > transform.position.x ? Vector2.right : Vector2.left;
//        Vector2 direction = (Vector2)Vector3.Normalize(player.position - this.transform.position);
//        GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
//        orb.GetComponent<Rigidbody2D>().velocity = direction * orbSpeed;
//    }

//   /* public void ChangeToBeatenUpSprite()
//    {
//        spriteRenderer.sprite = beatenUpSprite;
//    }
//   */




//   /* void FixedUpdate()
//    {
//        if (bossCurrentHealth < 1)
//            //GetComponent<CircleCollider2D>().enabled = false;
//            Destroy(this.gameObject);
//    } */
//}



//using System;
//using UnityEngine;
//using UnityEngine.UI;
//public class BossController : MonoBehaviour
//{
//    public GameObject orbPrefab;  // Assign the orb prefab in the inspector
//    public Transform player;      // Assign the player's transform in the inspector
//    public Sprite beatenUpSprite; // Assign the beaten up sprite in the inspector
//    public float orbSpeed = 5f;   // Speed of the orb
//    public float shootInterval = 2f; // Interval between shots
//    private float shootTimer = 0f;
//    private SpriteRenderer spriteRenderer;
//    public int bossHealth = 100;
//    private Animator bossAnimator;


//    void Start()
//    {
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        bossAnimator = GetComponent<Animator>();
//    }

//    void Update()
//    {
//        shootTimer += Time.deltaTime;

//        if (shootTimer >= shootInterval)
//        {
//            ShootOrb();
//            shootTimer = 0f;
//        }

//        bossAnimator.SetInteger("bossHealth", bossHealth);
//    }


//    void ShootOrb()
//    {
//        // Vector2 direction = player.position.x > transform.position.x ? Vector2.right : Vector2.left;
//        Vector2 direction = (Vector2)Vector3.Normalize(player.position - this.transform.position);
//        GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
//        orb.GetComponent<Rigidbody2D>().velocity = direction * orbSpeed;
//    }

//    /* public void ChangeToBeatenUpSprite()
//     {
//         spriteRenderer.sprite = beatenUpSprite;
//     }
//    */




//    public void TakeDamage(int damage)
//    {
//        bossHealth -= damage;



//    }

//    void FixedUpdate()
//    {
//        if (bossHealth < 1)
//            //GetComponent<CircleCollider2D>().enabled = false;
//            Destroy(this.gameObject);
//    }
//}

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

        // Update boss animation based on health
        bossAnimator.SetInteger("bossHealth", currentHealth);

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

        // Update the health bar UI
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Optional: Trigger death animation or state
        Destroy(this.gameObject);  // Destroy the boss object when health reaches 0
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        return;
    }
}