using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform groundCheckObj;
    [SerializeField] private AbilityToggleUI toggleUI;
    [Tooltip("Layer to define what is considered 'ground'")]
    public LayerMask groundLayer;
    private SpriteRenderer playerSpriteRenderer;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private RandomPitchAudioSource audioSource;

    [Header("Parameters")]
    public float maxHorizontalVelocity;
    [Tooltip("Force applied when jumping from the ground")]
    public float groundJumpForce = 9;
    [Tooltip("Factor applied to gravity when player is falling")]
    public float downGravityScaleFactor = 1.5f;
    private float originalGravityScale;
    [Tooltip("Maximum velocity when falling")]
    public float maxVerticalVelocity = 15f;
    public AudioClip groundJumpSFX;

    [Header("Parameters - Air Jump")]
    public bool isAirJumpSkillAcquired = true;
    [Tooltip("Force applied when air jumping")]
    public float airJumpForce = 9;
    [Tooltip("Includes ground jump and air jump(s)")]
    public int maxTotalNumberOfJumps = 2;
    public AudioClip airJumpSFX;

    [Header("State")]
    [Tooltip("Whether the player is considered to be 'on the ground'")]
    public bool isGrounded;
    [Tooltip("How many times the player has jumped since leaving the ground (inclusive)")]
    public int jumpCount = 0;


    public void Start()
    {
        toggleUI = FindFirstObjectByType<AbilityToggleUI>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<RandomPitchAudioSource>();
        jumpCount = 1; //To force grounded check at the start
        originalGravityScale = playerRb.gravityScale;
    }


    void Update()
    {
        CheckIfPlayerIsGrounded();
        ResetJumpCountIfGrounded();
        UpdateGravityScaleFactor();
        HandleHorizontalInput();
        HandleJumpInput();
        ClampVerticalVelocity();
        isAirJumpSkillAcquired = toggleUI.DoubleJump;
    }


    private void CheckIfPlayerIsGrounded()
    {
        bool playerIsAscending = playerRb.velocity.y > 0;
        isGrounded = !playerIsAscending && IsGroundCheckObjTouchingGroundLayer();
        playerAnimator.SetBool("isGrounded", isGrounded);
    }

    private bool IsGroundCheckObjTouchingGroundLayer()
    {
        return Physics2D.OverlapCircle(groundCheckObj.position, 0.2f, groundLayer) != null;
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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2(horizontalInput * maxHorizontalVelocity, playerRb.velocity.y);
        playerAnimator.SetBool("isWalking", horizontalInput != 0);
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
            if (isGrounded)
            {
                EffectJump(groundJumpForce, groundJumpSFX);
            }
            else if (isAirJumpSkillAcquired && (jumpCount < maxTotalNumberOfJumps))
            {
                EffectJump(airJumpForce, airJumpSFX);
            }
        }

        // Jump cutting - vertical velocity ends as soon as the player releases the jump button
        if(Input.GetButtonUp("Jump"))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Min(0, playerRb.velocity.y));
        }
    }

    private void EffectJump(float jumpForce, AudioClip clip)
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        jumpCount++;
        playerAnimator.SetTrigger("jumped");
        audioSource.PlayAudioWithRandomPitch(clip);
    }


    private void ClampVerticalVelocity()
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -maxVerticalVelocity, maxVerticalVelocity));
    }
}
