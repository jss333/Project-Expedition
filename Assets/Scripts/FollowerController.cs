using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
   [SerializeField] private GameObject anchor;
   public float follow_offset = 1F;
   public float chaseSpeed = 3F;
   [SerializeField] private float currentSpeed = 0;
   public float accelRate = 2F;
   private Rigidbody2D rb;
   [SerializeField] private Rigidbody2D R0Brb;

   void Start(){
        rb = GetComponent<Rigidbody2D>();
        if(R0Brb == null){
          Debug.Log("R0B rb2d missing...");
          this.gameObject.SetActive(false);
        }
        //anchor = transform.parent.gameObject;
   }

   void Update(){
        Vector2 dist_v = anchor.transform.position - transform.position;
        if(dist_v.magnitude > follow_offset){
               currentSpeed += accelRate * Time.deltaTime;
               if(currentSpeed > chaseSpeed){currentSpeed = chaseSpeed;}
        }
        else{
          currentSpeed -= accelRate * Time.deltaTime;
          if(currentSpeed < 0){currentSpeed = 0;}
        }
        rb.velocity = Vector3.Normalize(dist_v) * currentSpeed;
   }
}
