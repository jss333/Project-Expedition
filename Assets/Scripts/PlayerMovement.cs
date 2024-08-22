using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variables for player movement and jumping
    public Rigidbody2D playerRb;           // Rigidbody2D component to handle physics
    public float speed;                    // Speed of the player's horizontal movement
    public float jumpForce;                // Force applied when the player jumps
    public SpriteRenderer spriteRender;    // SpriteRenderer component to flip the player's sprite
    public Transform groundCheck;          // Transform to check if the player is grounded
    public LayerMask groundLayer;          // LayerMask to define what is considered ground
    private bool isGrounded;               // Boolean to check if the player is on the ground
    private int jumpCount;                 // Count of how many times the player has jumped
    public int maxJump = 2;                // Maximum number of jumps allowed (including double jump)

    // Variables for wall sliding and wall jumping
    public LayerMask wallLayer;            // LayerMask to define what is considered a wall
    public Transform wallCheck;            // Transform to check if the player is touching a wall
    public float wallSlideSpeed = 0.5f;    // Speed at which the player slides down a wall
    private bool isTouchingWall;           // Boolean to check if the player is touching a wall
    private bool isWallSliding;            // Boolean to check if the player is sliding down a wall
    private bool isFacingRight = true;     // Boolean to track the direction the player is facing
    public float wallJumpForce;            // Force applied when the player wall jumps
    private bool isWallJumping;            // Boolean to check if the player is in the middle of a wall jump

    void Update()
    {
        // Handle horizontal movement
        float input = Input.GetAxisRaw("Horizontal");

        // Flip the player's sprite based on the direction of movement
        if (input < 0)
        {
            spriteRender.flipX = true; // Face left
        }
        else if (input > 0)
        {
            spriteRender.flipX = false;  // Face right
        }

        // Apply horizontal movement if the player is not wall jumping
        if (!isWallJumping)
        {
            playerRb.velocity = new Vector2(input * speed, playerRb.velocity.y);
        }

        // Check if the player is grounded using OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Reset jump count when the player is grounded
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Check if the player is touching a wall using OverlapCircle
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

        // Handle wall sliding
        if (isTouchingWall && !isGrounded && playerRb.velocity.y < 0)
        {
            isWallSliding = true; // Player is sliding down the wall
            playerRb.velocity = new Vector2(playerRb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }

        // Handle jumping input
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                // Perform a regular jump when the player is grounded
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                jumpCount = 1; // Reset jump count after jumping from the ground
            }
            else if (isWallSliding)
            {
                // Perform a wall jump when the player is sliding down a wall
                isWallJumping = true;
                float wallJumpingDirection;

                // Determine the direction of the wall jump based on player's facing direction
                if (isFacingRight)
                {
                    wallJumpingDirection = -1; // Jump to the left
                }
                else
                {
                    wallJumpingDirection = 1;  // Jump to the right
                }

                // Apply the wall jump force
                playerRb.velocity = new Vector2(wallJumpingDirection * wallJumpForce, jumpForce);
                jumpCount = 1; // Allow for a double jump after the wall jump

                // Reset wall jump state after a short delay
                Invoke(nameof(ResetWallJump), 0.2f);
            }
            else if (jumpCount < maxJump)
            {
                // Perform a double jump if the player has jumps remaining
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                jumpCount++;
            }
        }
    }

    // Reset the wall jump state to allow normal movement
    private void ResetWallJump()
    {
        isWallJumping = false;
    }
}



