using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected InputHandler inputHandler;
    protected Animator animator;
    protected PlayerPawnController controller;
    
    public PlayerState(InputHandler inputHandler, PlayerStateMachine stateMachine, PlayerPawnController playerPawnController, Animator animator)
    {
        this.inputHandler = inputHandler;
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.controller = playerPawnController;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnUpdate()
    {
        //Debug.Log(this);
    }

    public virtual void OnExit()
    {
    }
}
