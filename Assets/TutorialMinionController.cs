using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMinionController : MonoBehaviour
{
    private Transform playerTransform;

    [Header("Health")]
    [SerializeField] private float collisionDmg = 5F;

    [Header("Attack")]
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private float attackPeriod = 2F;
    [SerializeField] private float attackTimer = 0;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float maxPlayerDistanceToShoot = 3f;

    public void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerMovement>().gameObject.transform;
    }

    public void Update()
    {
        Vector3 distanceToPlayer = GetDistanceToPlayer();

        if (PlayerIsCloseEnough(distanceToPlayer) && attackTimer >= attackPeriod)
        {
            attackTimer = 0;

            GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
            orb.GetComponent<Rigidbody2D>().velocity = GetDirectionToPlayer(distanceToPlayer) * speed;
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }

    private Vector3 GetDistanceToPlayer()
    {
        return playerTransform.position - this.transform.position;
    }

    private bool PlayerIsCloseEnough(Vector3 distanceToPlayer)
    {
        return distanceToPlayer.magnitude <= maxPlayerDistanceToShoot;
    }

    private Vector2 GetDirectionToPlayer(Vector3 distanceToPlayer)
    {
        return (Vector2)Vector3.Normalize(distanceToPlayer);
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealthComponent>().TakeDamage((int)collisionDmg);
        }
    }

    public void DestroyMinion()
    {
        Destroy(this.gameObject);
    }
}
