using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFireSys : MonoBehaviour
{
    private Rigidbody2D rb;
    public float despawnTime = 2F;
    private float timer = 0;
    public float speed = 2F;
    public int dmg = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < despawnTime){
            timer += Time.deltaTime;
        }
        else{
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            collision.gameObject.GetComponent<PlayerHealth>().TakehealthDamage(dmg);
            Destroy(this.gameObject);
        }
    }

    private void OnBecameInvisible(){
        Destroy(this.gameObject);
    }
}
