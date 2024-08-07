using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D playerRb;
    public float speed;
    public float input;
    public SpriteRenderer spriteRender;
    public float jumpForce;

    // Update is called once per frame
    void Update()
    {
        //horizontal movement of Robot
        input = Input.GetAxisRaw("Horizontal");

        // flipping Robot based on movements
        if(input < 0){
            spriteRender.flipX = false;
        }
        else if (input > 0){
            spriteRender.flipX = true;
        }

        //checking jump key is pressed (default - space Bar)
        if (Input.GetButton("Jump"))
        {
            playerRb.velocity = Vector2.up * jumpForce;
        }
    }

    //For smooth run of the robot
    void FixedUpdate()
    {
        playerRb.velocity = new Vector2(input * speed, playerRb.velocity.y);
    }
}
