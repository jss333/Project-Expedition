using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalMovementState : PlayerState
{
    public PlayerNormalMovementState(InputHandler inputHandler, PlayerStateMachine stateMachine, PlayerPawnController playerPawnController, Animator animator) : base(inputHandler, stateMachine, playerPawnController, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        inputHandler.controls.Std.Enable(); 
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //inputHandler.controls.Std.Enable();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
