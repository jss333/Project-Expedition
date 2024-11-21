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
        if (collision.GetComponent<HealthComponent>() != null && !collision.CompareTag("Player"))
         {
            collision.GetComponent<HealthComponent>().TakeDamage(10);
            Destroy(this.gameObject);
         }
    }

    private void OnDestroy()
    {
        Instantiate(onDestroyVFX,transform.position, transform.rotation);
    }

}