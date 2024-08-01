using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InputSys : MonoBehaviour
{
    public Rigidbody2d rb;
    public float max_vel = 2;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
