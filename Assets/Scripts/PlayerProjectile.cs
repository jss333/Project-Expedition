using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private int damageAmt = 2;


    public void SetVelocityAndDamageAmt(float speed, int damageAmt)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = this.transform.right * speed;
        this.damageAmt = damageAmt;
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
