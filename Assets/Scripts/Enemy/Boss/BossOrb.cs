using HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOrb : MonoBehaviour
{
    public int orbDmg = 5;
    private bool canDamageBossAndMinions;
    int playerLayerMask;
    int enemyLayerMask;
    int minionLayerMask;
    List<int> damagableLayersIndices = new List<int>();

    private void Start()
    {
        playerLayerMask = LayerMask.NameToLayer("Player");
        enemyLayerMask = LayerMask.NameToLayer("Boss");

        damagableLayersIndices.Add(playerLayerMask);
    }

    public void SetCanDamageBossAndMinions(bool _b)
    {
        canDamageBossAndMinions = _b;
        damagableLayersIndices.Add(enemyLayerMask);

    }

    public void SetNewDamagevalue(int _damageValue)
    {
        orbDmg = _damageValue;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(damagableLayersIndices.Contains(other.gameObject.layer))
        {
            other.GetComponent<Health>().TakeDamage(orbDmg);
        }

        /*if(canDamageBossAndMinions)
        {
            if (other.gameObject.tag == "Boss")
            {
                other.gameObject.GetComponent<BossController>().TakeDamage(orbDmg);
                Destroy(this.gameObject);
            }  
            
            if (other.gameObject.tag == "Minion")
            {
                other.gameObject.GetComponent<MinionController>().TakeDamage(orbDmg);
                Destroy(this.gameObject);
            }

            return;
        }

        if (other.gameObject.tag == "Player" && !canDamageBossAndMinions)
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(orbDmg);
            Destroy(this.gameObject);
        } */
    }
}
