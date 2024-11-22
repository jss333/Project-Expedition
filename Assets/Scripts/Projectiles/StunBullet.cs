using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBullet : MonoBehaviour
{
    [SerializeField] private int damage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            PlayerHealthComponent health = collision.GetComponent<PlayerHealthComponent>();
            if (health != null)
            {
                health.TakeDamage(damage);

                Destroy(this.gameObject);
            }

            IStunnable stunnable = collision.GetComponent<IStunnable>();
            
            if(stunnable != null)
            {
                stunnable.ApplyStunEffect();
            }
        }
    }
}
