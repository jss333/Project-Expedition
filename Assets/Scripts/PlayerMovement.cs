using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private SpriteRenderer playerSpriteRenderer;
    private Rigidbody2D playerRb;
    public Transform groundCheckObj;
    [Tooltip("Layer to define what is considered 'ground'")]
    public LayerMask groundLayer;

    [Header("Parameters")]
    public float maxHorizontalVelocity;
    [Tooltip("Force applied when jumping from the ground")]
    public float groundJumpForce = 9;
    [Tooltip("Factor applied to gravity when player is falling")]
    public float downGravityScaleFactor = 1.5f;
    private float originalGravityScale;
    [Tooltip("Maximum velocity when falling")]
    public float maxVerticalVelocity = 15f;

    [Header("Parameters - Air Jump")]
    public bool isAirJumpSkillAcquired = true;
    [Tooltip("Force applied when air jumping")]
    public float airJumpForce = 9;
    [Tooltip("Includes ground jump and air jump(s)")]
    public int maxTotalNumberOfJumps = 2;

    [Header("State")]
    [Tooltip("Whether the player is considered to be 'on the ground'")]
    public bool isGrounded;
    [Tooltip("How many times the player has jumped since leaving the ground (inclusive)")]
    public int jumpCount = 0;


    public void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerRb = GetComponent<Rigidbody2D>();
        jumpCount = 1; //To force grounded check at the start
        originalGravityScale = playerRb.gravityScale;
    }


    void Update()
    {
        UpdateGravityScaleFactor();
        ResetJumpCountTo0IfPlayerIsGrounded();
        HandleHorizontalInput();
        HandleJumpInput();
        ClampVerticalVelocity();
    }

    private void UpdateGravityScaleFactor()
    {
        if(playerRb.velocity.y < 0)
        {
            playerRb.gravityScale = originalGravityScale * downGravityScaleFactor;
        }
        else
        {
            playerRb.gravityScale = originalGravityScale;
        }
    }

    private void ResetJumpCountTo0IfPlayerIsGrounded()
    {
        if (jumpCount != 0)
        {
            if (IsPlayerGrounded())
            {
                jumpCount = 0;
            }
        }
    }

    private bool IsPlayerGrounded()
    {
        bool playerIsAscending = playerRb.velocity.y > 0;
        isGrounded = !playerIsAscending && IsGroundCheckObjTouchingGroundLayer();
        return isGrounded;
    }

    private bool IsGroundCheckObjTouchingGroundLayer()
    {
        return Physics2D.OverlapCircle(groundCheckObj.position, 0.2f, groundLayer) != null;
    }


    private void HandleHorizontalInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2(horizontalInput * maxHorizontalVelocity, playerRb.velocity.y);
        FlipSpriteBasedOnHorizontalInput(horizontalInput);
    }
    private void FlipSpriteBasedOnHorizontalInput(float horizontalInput)
    {
        if (horizontalInput == 0) return; //maintain previous orientation when there is no input
        playerSpriteRenderer.flipX = horizontalInput < 0;
    }


    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsPlayerGrounded())
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, groundJumpForce);
                jumpCount++;
            }
            else if (isAirJumpSkillAcquired && (jumpCount < maxTotalNumberOfJumps))
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, airJumpForce);
                jumpCount++;
            }
        }

        // Jump cutting - vertical velocity ends as soon as the player releases the jump button
        if(Input.GetButtonUp("Jump"))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Min(0, playerRb.velocity.y));
        }
    }

    private void ClampVerticalVelocity()
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -maxVerticalVelocity, maxVerticalVelocity));
    }
}
