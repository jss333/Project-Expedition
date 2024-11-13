using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControllerBase : MonoBehaviour
{
    public GameObject ControlledPawn { get; private set; }
    public GameStateBase GameState { get; private set; }
    public PlayerStateBase PlayerState { get; private set; }
    /*Passes Current controller, New Pawn and Old pawn, if old/new pawn is invalid returns null*/
    public readonly UnityEvent<PlayerControllerBase, GameObject, GameObject> OnPossessed = new UnityEvent<PlayerControllerBase, GameObject, GameObject>();
    /*Passes Current Controller, and old pawn*/
    public readonly UnityEvent<PlayerControllerBase, GameObject> OnUnpossessed = new UnityEvent<PlayerControllerBase, GameObject>();

    /* 
     * Pretty please do not Override this function!!!
     * Instead override Initialize which will be better.
     */
    public void PreInitialize(GameStateBase gamestate, PlayerStateBase playerState)
    {
        PlayerState = playerState;
        GameState = gamestate;

        Initialize();
    }

    protected void Initialize()
    {

    }

    public void  Possess(GameObject pawn)
    {
        if (!pawn)
        {
            Debug.LogWarning("Invalide pawn passed to be possessed.");
        }

        OnPossessed.Invoke(this, pawn, ControlledPawn);
        ControlledPawn = pawn;

    }

    public void Unpossess()
    {
        if(!ControlledPawn)
        {
            Debug.LogWarning("Invalid pawn, cannot unpossess if no pawn exists.");
        }

        OnUnpossessed.Invoke(this, ControlledPawn);
        ControlledPawn = null;        
    }
}
