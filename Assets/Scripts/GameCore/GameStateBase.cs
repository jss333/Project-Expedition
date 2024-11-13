using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// equivalent to Friend where GameModeBase has direct access to GameStates Internals.
[assembly: InternalsVisibleTo("GameModeBase")]

public class GameStateBase : MonoBehaviour
{
    

    List<PlayerStateBase> PlayerStates = new List<PlayerStateBase>();
    List<PlayerControllerBase> Players = new List<PlayerControllerBase>();

    public bool GameActive { get; private set; } = false;
    public string GameTitle { get; protected set; } = "Game Title";
    private float _gametime = float.PositiveInfinity;
    public float GameTime { get => Mathf.Max(Time.time - GameTime, 0.0f); private set => _gametime = value; }
    public GameModeBase GameMode { get; private set; }
    public readonly UnityEvent<PlayerControllerBase> OnPlayerAdded = new UnityEvent<PlayerControllerBase>();

    private void Start()
    {

        GameMode = FindFirstObjectByType<GameModeBase>(); //must exist
        if (!GameMode)
        {
            Debug.LogWarning("Could not find Game mode, will delete this object now as with out a game mode this object is pointless");
            Destroy(this);
            return;
        }

        PreInitalize();
    }

    private void PreInitalize()
    {
        Initialize();
    }
    protected void Initialize() {}

    public void StartMatch(string gameTitle = "")
    {
        if (GameActive)
        {
            return; //cant start a match that is already started.
        }
        GameTime = Time.time;
        GameActive = true;
        GameTitle = gameTitle == "" ? GameTitle : gameTitle;
    }

    public void AddPlayer(PlayerStateBase playerState, PlayerControllerBase playerController)
    {
        PlayerStates.Add(playerState);
        Players.Add(playerController);

        playerState.PreIntialize(this, playerController);
        playerController.PreInitialize(this, playerState);

        OnPlayerAdded.Invoke(playerController);
    }

    public void EndMatch(string reason)
    {
        if(!GameActive)
        {
            return; // cant start a match that is not started.
        }

        GameTime = float.PositiveInfinity; // so it will be 0
        GameActive = false;
    }
    

}
