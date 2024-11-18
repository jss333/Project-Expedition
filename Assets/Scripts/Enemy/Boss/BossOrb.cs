using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOrb : MonoBehaviour
{
    public int orbDmg = 5;
    private bool canDamageBossAndMinions;

    public void SetCanDamageBossAndMinions(bool _b)
    {
        canDamageBossAndMinions = _b;
    }

    public void SetNewDamagevalue(int _damageValue)
    {
        orbDmg = _damageValue;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(canDamageBossAndMinions)
        {
            if (!other.CompareTag("Player"))
            {
                other.GetComponent<HealthComponent>().TakeDamage(orbDmg);
                Destroy(this.gameObject);
            }
            return;
        }

        if (other.gameObject.tag == "Player" && !canDamageBossAndMinions)
        {
            other.GetComponent<HealthComponent>().TakeDamage(orbDmg);
            Destroy(this.gameObject);
        }
    }
}
