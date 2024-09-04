using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOrb : MonoBehaviour
{
    public int orbDmg = 5;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(orbDmg);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Out Of Bounds")
        {
            Destroy(this.gameObject);
        }
    }
}
