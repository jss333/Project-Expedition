using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombThrowController : MonoBehaviour
{
    [Header("Parameters")]
    private float bombSpeed;
    [SerializeField] private float duration;
    //[SerializeField] private int bombImpactDamage = 40;
    [SerializeField] private int bombExplosionDamage = 300;
    [SerializeField] private float bombExplosionRadius = 2f;
    [SerializeField] private AnimationCurve bombSpeedCurve;

    [Header("Variables")]
    private Vector2 spawnLocation;
    private Vector2 mouseLocation;

    private float timeSinceSpawn = 0;
    private float mouseDistance;
    private float sampleSpeed;

    private GameObject player;
    void Start()
    {
        spawnLocation = transform.position;
        mouseLocation = Input.mousePosition;
        player = FindAnyObjectByType<PlayerMovement>().gameObject;

        mouseDistance = Vector2.Distance(spawnLocation, mouseLocation);
        Debug.Log(mouseDistance);

        sampleSpeed = bombSpeedCurve.Evaluate(mouseDistance/2203);

        bombSpeed = sampleSpeed * 20;
        Debug.Log(bombSpeed);

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
                if(hit.tag == "Boss")
                {
                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercentCalc = Mathf.InverseLerp(bombExplosionRadius, 0, distance);

                    hit.GetComponent<BossController>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage));
                }
                else if(hit.tag == "Minion")
                {
                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercentCalc = Mathf.InverseLerp(bombExplosionRadius, 0, distance);

                    hit.GetComponent<MinionController>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage));
                }
            }
            //Gizmos.DrawSphere(transform.position, bombExplosionRadius);
        }
        Destroy(gameObject);
    }

    private void ThrowBomb()
    {
        GetComponent<Rigidbody2D>().AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * bombSpeed, ForceMode2D.Impulse);
    }
}
