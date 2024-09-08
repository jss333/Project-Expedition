using UnityEngine;

public class Projectile_Sys : MonoBehaviour
{
    [Header("Movement Properties")]
    public float speed = 20f;
    public int damageAmt = 2;


    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
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
