using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunState : PlayerState
{
    private float maxStuntime = 1.5f;
    private float stuntime;

    //private SpriteRenderer spriteRenderer;
    private SpriteRenderer[] spriteRenderers;

    public PlayerStunState(InputHandler inputHandler, PlayerStateMachine stateMachine, PlayerPawnController playerPawnController, Animator animator) : base(inputHandler, stateMachine, playerPawnController, animator)
    {
        //spriteRenderer = playerPawnController.GetComponent<SpriteRenderer>();
        spriteRenderers = playerPawnController.transform.parent.GetComponentsInChildren<SpriteRenderer>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        inputHandler.controls.Std.Disable();

        stuntime = 0;

        //spriteRenderer.color = Color.blue;

        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.color = Color.cyan;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //inputHandler.controls.Std.Disable();

        stuntime += Time.deltaTime;
        if(stuntime > maxStuntime)
        {
            controller.TranslateToNormalState();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        stuntime = 0;

        //spriteRenderer.color = Color.white;

        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.color = Color.white;
        }
    }
}
