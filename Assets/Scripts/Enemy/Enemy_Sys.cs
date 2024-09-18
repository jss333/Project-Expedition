using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sys : MonoBehaviour
{
    public int Health = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Health < 1)
            //GetComponent<CircleCollider2D>().enabled = false;
            Destroy(this.gameObject);
    }
}
