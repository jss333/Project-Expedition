using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawnController : MonoBehaviour , IStunnable
{
    public PlayerNormalMovementState playerNormalMovementState;

    public PlayerStunState playerStunState;  

    public PlayerStateMachine playerStateMachine;

    [Header("Refs")]
    [SerializeField] private Animator animator;

    private InputHandler inputHandler;

    private void Start()
    {
        inputHandler = FindFirstObjectByType<InputHandler>();

        playerStateMachine = new PlayerStateMachine();

        playerNormalMovementState = new PlayerNormalMovementState(inputHandler, playerStateMachine, this, animator);
        playerStunState = new PlayerStunState(inputHandler, playerStateMachine, this, animator);

        playerStateMachine.Initialize(playerNormalMovementState);
    }

    private void Update()
    {
        playerStateMachine.currentState.OnUpdate();
    }

    [ContextMenu("Stun")]
    public void TranslateToStunState()
    {
        if (IsAlreadyStunned()) return;
        playerStateMachine.ChangeState(playerStunState);
    }

    [ContextMenu("Normal")]
    public void TranslateToNormalState()
    {
        playerStateMachine.ChangeState(playerNormalMovementState);
    }

    public void ApplyStunEffect()
    {
        TranslateToStunState();
    }

    private bool IsAlreadyStunned()
    {
        return playerStateMachine.currentState == playerStunState;
    }
}
