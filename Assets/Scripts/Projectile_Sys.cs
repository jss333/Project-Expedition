using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Sys : MonoBehaviour
{
    public Team[] TargetTeams = new Team[5];
    private GameObject TrackEntity = null;
    
    [Header("Movement Properties")]
    public float pos_vel = 2F;
    public float pos_acc = 30F;
    public float rot_vel = 5F;
    public float rot_acc = 180F;
    public ProjectileType type = ProjectileType.Precision;
    public int damageAmt = 5;

    private Rigidbody2D rb;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        TrackEntity = FindNearestTarget();
    }
    
    void FixedUpdate()
    {
        if(type == ProjectileType.Tracking && TrackEntity != null){
            GuideTrackingProjectile();
        }
        else{
            GuidePrecisionProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("Entered collision...");
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<BossController>().bossHealth -= 1;

            Destroy(this.gameObject);

        }
        else if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<BossController>().TakeDamage(damageAmt);
        }
    }

    void OnBecameInvisible(){
        Debug.Log("Self-destruct...");
        Destroy(this.gameObject);
    }

    void GuidePrecisionProjectile(){
        //rb.AddForce(transform.right * pos_acc);
        rb.velocity = this.transform.right * pos_vel;
    }

    void GuideTrackingProjectile(){
        //Aligns Tracker velocity vec with vec pointing at Target from Tracker by applying Torque
        Vector2 delta = TrackEntity.transform.position - this.transform.position;
        float AngularDist = Vector2.SignedAngle(delta, rb.velocity);
        if(AngularDist > 0){
            rb.AddTorque(-AngularDist);
        }
        else if(AngularDist < 0){
            rb.AddTorque(-AngularDist);
        }
        
        //rb.AddForce(this.transform.right * pos_acc);
        rb.velocity = this.transform.right * pos_vel;
    }

    private GameObject FindNearestTarget(){
        //Need to adjust the hierarchy before generalizing this. Consult with Rakshaan.
        GameObject Target = GameObject.Find("Boss");
        if(Target == null){
            Debug.Log("Couldn't find target...");
        }
        return Target;
    }
}

public enum ProjectileType{
    Precision,
    Tracking
}