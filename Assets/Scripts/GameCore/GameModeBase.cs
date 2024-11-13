using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeBase : MonoBehaviour
{
    /* 
     * Forced binded stuff that always happens on start,
     * This will set up binds to required objects for base games set
     * Addtiional initalization steps can be done on a later flow step
     */
    [SerializeField] protected LoadScreenManager LoadScreen;
    public GameInstanceBase GameInstance { get; private set; }
    [SerializeField] private GameStateBase GameState;
    [SerializeField] private PlayerStateBase  DefaultPlayerState;
    [SerializeField] private PlayerControllerBase DefaultPlayerController;
    [SerializeField] private GameObject DefaultPlayerPawn;



    private void Start() // alwayed used when initalizing. so object should be destroyed if needs reset.
    { 
        PreInitalize();
    }

    /*
     * Pre initalization for object this handdles such things as binding and dealing 
     * with general data flows that all game modes need, custom code should be added
     * to an inherited model.
     */
    private void PreInitalize()
    {
        if (!LoadScreen)
        {
            Debug.LogWarning("Please define Load screen so we have the correct loading screen.");
            GameObject GameOBJ = new GameObject("Please Replace With Load Screen PREFAB");
            LoadScreen = GameOBJ.AddComponent<LoadScreenManager>();
        }
        else
        {
            LoadScreen = Instantiate(LoadScreen);
        }
        LoadScreen.Show();

        GameInstance = FindFirstObjectByType<GameInstanceBase>(); //must exist
        if (!GameInstance)
        {
            Debug.LogWarning("Could not find Game instance from game mode. Unclear how Game Mode was spawned but should not exist. Some persistant data funcationality may not work correctly");
        }

        if(!GameState)
        {
            Debug.LogWarning("Please define Game State so we have the correct Game data.");
            GameObject GameOBJ = new GameObject("Please Replace With Game State PREFAB");
            GameState = GameOBJ.AddComponent<GameStateBase>();
        }
        else
        {
            GameState = Instantiate(GameState);
        }

        if (!DefaultPlayerState)
        {
            Debug.LogWarning("Please define the default player state so we can track player data.");
            GameObject GameOBJ = new GameObject("Please Replace With Player State PREFAB");
            DefaultPlayerState = GameOBJ.AddComponent<PlayerStateBase>();
        }

        if (!DefaultPlayerController)
        {

        }
        //GameState Instantiate(PlayerState);
        
        AddPlayer();
        Initalize();

    }

    private void AddPlayer()
    {
        PlayerControllerBase playerController = Instantiate(DefaultPlayerController);
        PlayerStateBase playerState = Instantiate(DefaultPlayerState);

        //Spawn in default pawn and assign it to the controller
        //Update the controller with into about the player
        GameObject pawn = Instantiate(DefaultPlayerPawn);
        playerController.Possess(pawn);

        GameState.AddPlayer(playerState, playerController);
    }
    protected void Initalize()
    {
        GameState.StartMatch("");
        LoadScreen.Hide();
    }

    /*
     * Wrap unity command in protector so we can do required steps first
     * then later use shut down for per instance version of this function
     */

    private void OnDestroy()
    {
        Destroy(LoadScreen); //clean up owned objects.
        Shutdown();
    }

    /* 
     * Instance editable version which allows users to run custom code before a
     * game is reset. This could be due to a crash / stall / soft lock, could be from
     * a user request, or from an exit of the game.
     */
    protected void Shutdown()
    {
    }
}
