using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * This is the start point of the game, and should be the core initializer in the game
 * This class handles high level persistant functionality and should never be destroyed.
 * This class manages spawning the game state manager (game mode) and also manages handling
 * the flow of save data, achievements, meta data, telemetrics, and any data that is persistant
 * THis class should not directly manage any of these systems but rather spawn and rediect to 
 * thes script objects.
 */

public class GameInstanceBase : MonoBehaviour
{
    [SerializeField] private GameModeBase GameMode;
    //public GameObject GameModePrefab; 
    
    //SaveGame []
    //local controller // multiplayer maybe

    /* 
     * Intialize this object so that it can not be used at lower levels
     * This will force people to use the Initalize comand line allowing for 
     * base level functionality to always be preformed.
     */
    private void Start() 
    {
        //make this object persistant so we can use it as an anchor
        DontDestroyOnLoad(this.gameObject);
        PreInitalize();
    }
    /*
     * Used to (re)Initalize the game state, this can be used for a return to main menu, 
     * or for game launch. This will reset the game Mode which will reset all sub objects:
     * player, controller,  game state, game managers etc
     */
    private void PreInitalize()
    {
        if (!this.GameMode)
        {
            Debug.LogError("Game Mode Is not bound, this will result in inproper game state.");
            return;
        }

        this.GameMode = Instantiate(GameMode);
        //GameMode.Start();
        //impicited call by instantiating.

        //Set up a shut down call so we can clean up libraries and such as needed.
        Application.quitting += this.Shutdown;

        //call internal open call for additional specialization for current game.
        Initialize();
    }


    /* 
     * Used on application initalization before any object is started
     * Can be useful for updating and crerating data flows for save data or persistant user data
     * Additionally can be used to get player stats or Web formated data calls for tuning, or other data usage.
    */
    protected void Initialize() {}

    /*
     * Get base levle info on a save slot to be displayed to allow user to load a slot
     */
    public string  ReadSaveSlot(string slot)
    {
        return ""; 
        //will return a structure of what is in the slot data, this would be stuff like time created,
        //time modified, progression etc some of this data will be jsonified and be in the header of the file
        //allowing for quick acess.
    }

    /*
     * Gets the data in the slot and returns it back, this would start the actual loading process of
     * the data as well so this may trigger other events to fire.
     */
    public string LoadSlot(string slot)
    {
        return "";
    }

    /*
     * Updates slot listed with data given which should be in json format
     * If partial is true, this will add to current data (overriding duplicate values). 
     */ 
    public bool SaveToSlot(string slot, string data, bool partial)
    {
        return false;
    }

    /*
    * Function handler for closing applicaitons
    * This can be used to clean up save data or get gener information.
    */
    protected void Shutdown() {}
}
