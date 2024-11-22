using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase : MonoBehaviour
{
    public GameStateBase GameState { get; private set; }
    public PlayerControllerBase PlayerController { get; private set; }
    public GameObject ControlledPawn { get => PlayerController ? PlayerController.ControlledPawn : null; }
    
    /* 
     * Pretty please do not Override this function!!!
     * Instead override Initialize which will be better.
     */
    public void PreIntialize(GameStateBase gamestate, PlayerControllerBase playerController)
    {
        PlayerController = playerController;
        GameState = gamestate;

        Initialize();
    }

    protected void Initialize()
    {

    }
}
