using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public LayerMask layersToCollideWith;
    public int damageAmt = 2;
    public float maxRange;

    float timeAlive;
    float maxSpeed;
    float distanceTravelled;

    public void SetVelocityAndDamageAmt(float speed, int damageAmt)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        maxSpeed = speed; 
        rb.velocity = this.transform.right * speed;
        this.damageAmt = damageAmt;
    }

    private void Update()
    {
        if (maxRange == 0) return;

        timeAlive += Time.deltaTime;

        distanceTravelled = timeAlive * maxSpeed;

        if (distanceTravelled > maxRange)
        {
            Destroy(gameObject);
        }
    }

    /*if(layersToCollideWith == (layersToCollideWith | (1 << other.gameObject.layer)))
    {
        if (other.gameObject.tag == "Boss")
        {
            BossController boss = other.gameObject.GetComponent<BossController>();
            boss.TakeDamage(damageAmt);
        }
        Destroy(this.gameObject);
    }
}*/

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
