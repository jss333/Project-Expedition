using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform groundCheckObj;
    private AbilityToggleUI toggleUI;
    [SerializeField] private EventReference playerGroundJumpSFX;
    [SerializeField] private EventReference playerAirJumpSFX;

    [Tooltip("Layer to define what is considered 'ground'")]
    public LayerMask groundLayer;
    public LayerMask bossLayer;
    private SpriteRenderer playerSpriteRenderer;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

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

    private float movementValue;

    [Header("Bomb")]
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject bombSticky;
    [SerializeField] private float bombCooldown = 1;
    [SerializeField] private float stickyBombCooldown = 1;
    private float canFireBomb = 0;
    private float canFireBombSticky = 0;

    public void Start()
    {
        toggleUI = FindFirstObjectByType<AbilityToggleUI>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        jumpCount = 1; //To force grounded check at the start
        originalGravityScale = playerRb.gravityScale;

        InputHandler.Singleton.OnPlayerMovementHandle += HandelMovement;
        InputHandler.Singleton.OnJumpDown += JumpDown;
        InputHandler.Singleton.OnJumpUp += JumpUp;
        InputHandler.Singleton.OnThrowBomb += ThrowBomb;
        InputHandler.Singleton.OnThrowStickyBomb += ThrowStickyBomb;
    }

    private void OnDestroy()
    {
        InputHandler.Singleton.OnPlayerMovementHandle -= HandelMovement;
        InputHandler.Singleton.OnJumpDown -= JumpDown;
        InputHandler.Singleton.OnJumpUp -= JumpUp;
        InputHandler.Singleton.OnThrowBomb -= ThrowBomb;
        InputHandler.Singleton.OnThrowStickyBomb -= ThrowStickyBomb;
    }


    void Update()
    {
        CheckIfPlayerIsGrounded();
        ResetJumpCountIfGrounded();
        UpdateGravityScaleFactor();
        HandleHorizontalInput();
        //HandleJumpInput();
        ClampVerticalVelocity();
        //isAirJumpSkillAcquired = toggleUI.DoubleJump;

        
    }


    private void CheckIfPlayerIsGrounded()
    {
        bool playerIsAscending = playerRb.velocity.y > 0;
        isGrounded = !playerIsAscending && IsGroundCheckObjTouchingGroundLayer();
        playerAnimator.SetBool("isGrounded", isGrounded);
    }

    private bool IsGroundCheckObjTouchingGroundLayer()
    {
        return Physics2D.OverlapCircle(groundCheckObj.position, 0.2f, groundLayer) != null 
            || Physics2D.OverlapCircle(groundCheckObj.position, 0.2f, bossLayer) != null;
    }


    private void ResetJumpCountIfGrounded()
    {
        if (jumpCount != 0)
        {
            if (isGrounded)
            {
                jumpCount = 0;
            }
        }
    }

    private void UpdateGravityScaleFactor()
    {
        if(playerRb.velocity.y < 0)
        {
            playerRb.gravityScale = originalGravityScale * downGravityScaleFactor;
            playerAnimator.SetBool("isFalling", true);
        }
        else
        {
            playerRb.gravityScale = originalGravityScale;
            playerAnimator.SetBool("isFalling", false);
        }
    }

    private void HandleHorizontalInput()
    {
        float horizontalInput = GetMovement();
        playerRb.velocity = new Vector2(horizontalInput * maxHorizontalVelocity, playerRb.velocity.y);
        playerAnimator.SetBool("isWalking", horizontalInput != 0);
        FlipSpriteBasedOnHorizontalInput(horizontalInput);
    }
    private void FlipSpriteBasedOnHorizontalInput(float horizontalInput)
    {
        if (horizontalInput == 0) return; //maintain previous orientation when there is no input
        playerSpriteRenderer.flipX = horizontalInput < 0;
    }

    private void HandelMovement(float movement)
    {
        movementValue = movement;
    }

    private float GetMovement()
    {
        return movementValue;
    }

    private void HandleJumpInput()
    {
        JumpDown();

        // Jump cutting - vertical velocity ends as soon as the player releases the jump button
        JumpUp();
    }

    private void JumpUp()
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Min(0, playerRb.velocity.y));
    }

    private void JumpDown()
    {
        if (isGrounded)
        {
            EffectJump(groundJumpForce, playerGroundJumpSFX);
        }
        else if (isAirJumpSkillAcquired && (jumpCount < maxTotalNumberOfJumps))
        {
            EffectJump(airJumpForce, playerAirJumpSFX);
        }
    }

    private void EffectJump(float jumpForce, EventReference sfx)
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        jumpCount++;
        playerAnimator.SetTrigger("jumped");
        AudioManagerNoMixers.Singleton.PlayOneShot(sfx, this.transform.position);
    }


    private void ClampVerticalVelocity()
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -maxVerticalVelocity, maxVerticalVelocity));
    }

    //this is a temp bomb spawning function
    private void ThrowBomb()
    {
        if(canFireBomb <= Time.time)
        {
            Instantiate(bomb, transform.position, Quaternion.identity);
            canFireBomb = Time.time + bombCooldown;
        }
    }
    private void ThrowStickyBomb()
    {
        if (canFireBombSticky <= Time.time)
        {
            Instantiate(bombSticky, FindFirstObjectByType<FollowerController>().transform.position, Quaternion.identity);
            canFireBombSticky = Time.time + stickyBombCooldown;
        }
    }

}
