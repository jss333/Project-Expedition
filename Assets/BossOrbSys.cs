using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOrb : MonoBehaviour
{
    public int orbDmg = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<PlayerHealth>().TakehealthDamage(orbDmg);
            Destroy(this.gameObject);
        }
    }
}
