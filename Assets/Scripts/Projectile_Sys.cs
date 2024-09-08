using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Sys : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject trackedEntity = null;

    [Header("Movement Properties")]
    public ProjectileType type = ProjectileType.Precision;
    public float speed = 20f;
    public int damageAmt = 2;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trackedEntity = FindNearestTarget();
    }
    
    void FixedUpdate()
    {
        if(type == ProjectileType.Tracking && trackedEntity != null){
            GuideTrackingProjectile();
        }
        else{
            GuidePrecisionProjectile();
        }
    }

    private GameObject FindNearestTarget()
    {
        //Need to adjust the hierarchy before generalizing this. Consult with Rakshaan.
        GameObject Target = GameObject.Find("Boss");
        if (Target == null)
        {
            Debug.Log("Couldn't find target...");
        }
        return Target;
    }

    void GuideTrackingProjectile()
    {
        //Aligns Tracker velocity vec with vec pointing at Target from Tracker by applying Torque
        Vector2 distanceToTargetFromProjectile = trackedEntity.transform.position - this.transform.position;
        float angleToTarget = Vector2.SignedAngle(distanceToTargetFromProjectile, rb.velocity);

        if (angleToTarget != 0)
        {
            rb.AddTorque(-angleToTarget);
        }
        else if (angleToTarget < 0)
        {
            rb.AddTorque(-angleToTarget);
        }

        rb.velocity = this.transform.right * speed;
    }

    void GuidePrecisionProjectile()
    {
        rb.velocity = this.transform.right * speed;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boss")
        {
            BossController boss = other.gameObject.GetComponent<BossController>();
            boss.TakeDamage(damageAmt);
            Destroy(this.gameObject);
        }
    }
}


public enum ProjectileType
{
    Precision,
    Tracking
}
