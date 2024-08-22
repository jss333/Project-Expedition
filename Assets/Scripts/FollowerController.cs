using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
   [SerializeField] private GameObject anchor;
   public float follow_offset = 1F;
   public float chase_speed = 3F;
   private Rigidbody2D rb;

   void Start(){
        rb = GetComponent<Rigidbody2D>();
        //anchor = transform.parent.gameObject;
   }

   void Update(){
        Vector2 dist_v = anchor.transform.position - transform.position;
        if(dist_v.magnitude > follow_offset){
            rb.AddForce(Vector3.Normalize(dist_v) * chase_speed);
        }
   }
}
