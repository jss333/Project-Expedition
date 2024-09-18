using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
  [Header("References")]
   [SerializeField] private GameObject anchor;
   private Rigidbody2D rb;
   [SerializeField] private Rigidbody2D R0Brb;

   [Header("Parameters")]
   public float follow_offset = 1F;
   public float chaseSpeed = 3F;
   [SerializeField] private float currentSpeed = 0;
   public float accelRate = 2F;
   

   void Start(){
        rb = GetComponent<Rigidbody2D>();
        //Verification of script functionality...
        if(R0Brb == null || anchor == null){
          Debug.Log("R0B rb2d missing...");
          this.gameObject.SetActive(false);
        }
   }

   void Update(){
        FollowR0B();
   }

   private void FollowR0B(){
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
