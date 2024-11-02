using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrowController : MonoBehaviour
{
    private Vector3 spawnLocation;
    private Vector2 mouseLocation;

    [Header("Parameters")]
    [SerializeField] private float bombSpeed;
    [SerializeField] private float duration;
    [SerializeField] private int bombImpactDamage = 40;
    [SerializeField] private int bombExplosionDamage = 300;
    [SerializeField] private float bombExplosionRadius = 1.2f;

    [Header("Variables")]
    private float timeSinceSpawn = 0;
    void Start()
    {
        spawnLocation = transform.position;
        mouseLocation = Input.mousePosition;

        ThrowBomb();
    }

    private void Update()
    {
        //alive duration
        if (timeSinceSpawn < duration)
        {
            timeSinceSpawn += Time.deltaTime;
        }
        else
        {
            Explode();
        }
    }
    private void Explode()
    {
        //code a radius for the explosion damage
        if(bombExplosionRadius > 0)
        {
            //check radius
            var hitEnemies = Physics2D.OverlapCircleAll(transform.position, bombExplosionRadius);

            //for each in radius of explosion deal damage
            foreach(var hit in hitEnemies)
            {
                if(hit.tag == "Enemy")
                {
                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercentCalc = Mathf.InverseLerp(bombExplosionRadius, 0, distance);

                    hit.GetComponent<BossController>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage));
                    Debug.Log("Explosion Deals " + (int)(damagePercentCalc * bombExplosionDamage));
                }
            }
            Gizmos.DrawSphere(transform.position, bombExplosionRadius);
        }
        //Destroy(gameObject);
    }

    private void ThrowBomb()
    {
        GetComponent<Rigidbody2D>().AddForce(mouseLocation * bombSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.GetComponent<BossController>() != null)
            {
                collision.gameObject.GetComponent<BossController>().TakeDamage(bombImpactDamage);
            }
            else if(collision.gameObject.GetComponent<MinionController>() != null)
            {
                collision.gameObject.GetComponent<MinionController>().TakeDamage(bombImpactDamage);
            }
            
        }
    }
}
