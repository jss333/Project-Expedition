using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer spriteRender;    // SpriteRenderer component to flip the player's sprite
    public Rigidbody2D playerRb;           // Rigidbody2D component to handle physics
    public Transform groundCheck;          // Transform to check if the player is grounded
    public LayerMask groundLayer;          // LayerMask to define what is considered ground
    public Transform wallCheck;            // Transform to check if the player is touching a wall
    public LayerMask wallLayer;            // LayerMask to define what is considered a wall

    [Header("Parameters")]
    public float maxHorizontalVelocity;                    // Speed of the player's horizontal movement
    public float jumpForce;                // Force applied when the player jumps
    public int maxJump = 2;                // Maximum number of jumps allowed (including double jump)
    public float wallSlideSpeed = 0.5f;    // Speed at which the player slides down a wall
    public float wallJumpForce;            // Force applied when the player wall jumps

    [Header("State")]
    public bool isGrounded;               // Boolean to check if the player is on the ground
    public bool isTouchingWall;           // Boolean to check if the player is touching a wall
    public bool isWallSliding;            // Boolean to check if the player is sliding down a wall
    public bool isWallJumping;            // Boolean to check if the player is in the middle of a wall jump
    public bool isFacingRight = true;     // Boolean to track the direction the player is facing
    public int jumpCount;                 // Count of how many times the player has jumped
 

    void Update()
    {
        // Handle horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        flipSpriteBasedOnHorizontalInput(horizontalInput);

        isGrounded = CheckIfPlayerIsGrounded();
        isTouchingWall = CheckIfPlayerIsTouchingWall();
        isWallSliding = isTouchingWall && !isGrounded && playerRb.velocity.y < 0;

        // Apply horizontal movement if the player is not wall jumping
        if (!isWallJumping)
        {
            playerRb.velocity = new Vector2(horizontalInput * maxHorizontalVelocity, playerRb.velocity.y);
        }

        // Handle wall sliding
        if (isWallSliding)
        {  
            playerRb.velocity = new Vector2(playerRb.velocity.x, -wallSlideSpeed);
        }

        // Reset jump count when the player is grounded
        if (isGrounded)
        {
            jumpCount = 0;
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

    private Collider2D CheckIfPlayerIsTouchingWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private bool CheckIfPlayerIsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void flipSpriteBasedOnHorizontalInput(float horizontalInput)
    {
        if (horizontalInput < 0)
        {
            spriteRender.flipX = true; // Face left
        }
        else if (horizontalInput > 0)
        {
            spriteRender.flipX = false;  // Face right
        }
    }

    // Reset the wall jump state to allow normal movement
    private void ResetWallJump()
    {
        isWallJumping = false;
    }
}



