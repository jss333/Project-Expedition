using UnityEngine;

public class Projectile_Sys : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement Properties")]
    public float speed = 20f;
    public int damageAmt = 2;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        GuidePrecisionProjectile();
    }

    void GuidePrecisionProjectile()
    {
        rb.velocity = this.transform.right * speed;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boss")
        {
            BossController boss = other.gameObject.GetComponent<BossController>();
            boss.TakeDamage(damageAmt);
            Destroy(this.gameObject);
        }
    }
}
