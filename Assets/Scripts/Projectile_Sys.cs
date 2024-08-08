using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Sys : MonoBehaviour
{
    public string TargetTag = "Player";
    public Vector2 velocity = Vector2.zero;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        this.transform.position += (Vector3)velocity * Time.deltaTime;
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
}
