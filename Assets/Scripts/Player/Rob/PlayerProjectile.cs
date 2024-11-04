using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public LayerMask layersToCollideWith;
    public int damageAmt = 2;
    public float maxRange;
    public GameObject onDestroyVFX;

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

    

    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (layersToCollideWith == (layersToCollideWith | (1 << collision.gameObject.layer)))
        {
            Destroy(this.gameObject);
        }
            /*if (collision.collider.gameObject.tag == "Boss")
            {
                BossController boss = collision.collider.gameObject.GetComponent<BossController>();
                boss.TakeDamage(damageAmt);
                Destroy(this.gameObject);
            }  */

            // if (layersToCollideWith == (layersToCollideWith | (1 << collision.gameObject.layer)))
            // {
            /*if (collision.gameObject.tag == "Boss")
            {
                BossController boss = collision.gameObject.GetComponent<BossController>();
                boss.TakeDamage(damageAmt);
            }  */
            //Destroy(this.gameObject);
            // }
    }

    private void OnDestroy()
    {
        Instantiate(onDestroyVFX,transform.position, transform.rotation);
    }

}