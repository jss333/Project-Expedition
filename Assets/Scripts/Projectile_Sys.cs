using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Sys : MonoBehaviour
{
    public string TargetTag = "Player";
    public float pos_vel = 1F;
    public float rot_vel = 2F;
    public ProjectileType type = ProjectileType.Precision;
    public Rigidbody2D rb;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if(type == ProjectileType.Tracking){
            GuideTrackingProjectile();
        }
        else{
            GuidePrecisionProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("Entered collision...");
        if(collision.gameObject.tag == TargetTag){
            collision.gameObject.GetComponent<Enemy_Sys>().Health -=1;
            Destroy(this.gameObject);
        }
    }

    void OnBecameInvisible(){
        Debug.Log("Self-destruct...");
        Destroy(this.gameObject);
    }

    void GuidePrecisionProjectile(){
        transform.position += transform.right * pos_vel;
    }

    void GuideTrackingProjectile(){

    }

    private GameObject findNearestTarget(){
        return null;
    }
}

public enum ProjectileType{
    Precision,
    Tracking
}