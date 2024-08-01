using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InputSys : MonoBehaviour
{
    public Rigidbody2D rb;
    public float max_vel = 2;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
