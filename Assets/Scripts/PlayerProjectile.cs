using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public LayerMask layersToCollideWith;
    private int damageAmt = 2;


    public void SetVelocityAndDamageAmt(float speed, int damageAmt)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = this.transform.right * speed;
        this.damageAmt = damageAmt;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(layersToCollideWith == (layersToCollideWith | (1 << other.gameObject.layer)))
        {
            if (other.gameObject.tag == "Boss")
            {
                BossController boss = other.gameObject.GetComponent<BossController>();
                boss.TakeDamage(damageAmt);
            }

            Destroy(this.gameObject);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.gameObject.tag == "Boss")
    //    {
    //        BossController boss = collision.collider.gameObject.GetComponent<BossController>();
    //        boss.TakeDamage(damageAmt);
    //        Destroy(this.gameObject);
    //    }
    //}
}
