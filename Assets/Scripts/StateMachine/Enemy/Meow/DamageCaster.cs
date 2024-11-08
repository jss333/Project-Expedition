using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        if(playerHealth != null )
        {
            playerHealth.TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
