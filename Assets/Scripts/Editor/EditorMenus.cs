using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorMenus : EditorWindow
{
    [MenuItem("LevelTools/Core Objects/Setup Level Objects", false, 0)]
    public static void CreateAllObjects()
    {
        CreateCameraSystem();
        CreatePauseMenu();
        CreateDebugMenu();
        CreatePlayer();
        CreateBoss();
        CreateAudioManager();
        CreateAbilityManager();
        CreateInputHandler();
    }




    [MenuItem("LevelTools/Core Objects/Create Audio Manager")]
    public static void CreateAudioManager()
    {
        GameObject audioManagerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Audio/Challenge Room BGM.prefab", typeof(GameObject));
        if (audioManagerPrefab)
        {
            GameObject audioManagerObject = GameObject.Instantiate(audioManagerPrefab);
            audioManagerObject.name = "AudioManager";
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the AudioManager Prefab!");
        }
    }

    [MenuItem("LevelTools/Core Objects/Create Player")]
    public static void CreatePlayer()
    {
        GameObject playerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Player/R0B-B3RT.prefab", typeof(GameObject));
        if (playerPrefab)
        {
            GameObject playerObject = GameObject.Instantiate(playerPrefab);
            playerObject.name = "Player";
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Player Prefab!");
        }
    }

    [MenuItem("LevelTools/Core Objects/Create Ability Manager")]
    public static void CreateAbilityManager()
    {
        GameObject abilityManagerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/AbilitySystem/AbilityManager.prefab", typeof(GameObject));
        if (abilityManagerPrefab)
        {
            GameObject abilityManagerObject = GameObject.Instantiate(abilityManagerPrefab);
            abilityManagerObject.name = "AbilityManager";
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Ability Manager Prefab!");
        }
    }

    [MenuItem("LevelTools/Core Objects/Create Input Handler")]
    public static void CreateInputHandler()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/InputHandler/InputHandler.prefab", typeof(GameObject));
        if (prefab)
        {
            GameObject newGameObject = GameObject.Instantiate(prefab);
            newGameObject.name = "InputHandler";
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Input Handler Prefab!");
        }
    }

    [MenuItem("LevelTools/Core Objects/Create Boss")]
    public static void CreateBoss()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemy/Boss.prefab", typeof(GameObject));
        if (prefab)
        {
            GameObject newGameObject = GameObject.Instantiate(prefab);
            newGameObject.name = "Boss";
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Boss Prefab!");
        }
    }

    [MenuItem("LevelTools/Core Objects/Create Camera System")]
    public static void CreateCameraSystem()
    {
        CreateCameraObject();

        CreateVirtualCamera();
    }

    public static void CreateCameraObject()
    {
        GameObject oldCamera = GameObject.FindObjectOfType<Camera>().gameObject;
        DestroyImmediate(oldCamera);

        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Camera/Main Camera.prefab", typeof(GameObject));
        if (prefab)
        {
            GameObject newGameObject = GameObject.Instantiate(prefab);
            newGameObject.name = "Main Camera";
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Main Camera Prefab!");
        }
    }

    public static void CreateVirtualCamera()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Camera/Virtual Camera.prefab", typeof(GameObject));
        if (prefab)
        {
            GameObject newGameObject = GameObject.Instantiate(prefab);
            newGameObject.name = "Virtual Camera";
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Virtual Camera Prefab!");
        }
    }


    [MenuItem("LevelTools/Core Objects/Debug Menu")]
    public static void CreateDebugMenu()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UI/DebugMenu.prefab", typeof(GameObject));
        if (prefab)
        {
            GameObject newGameObject = GameObject.Instantiate(prefab);
            newGameObject.name = "DebugMenu";
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Debug Menu Prefab!");
        }
    }

    [MenuItem("LevelTools/Core Objects/Pause Menu")]
    public static void CreatePauseMenu()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UI/PauseMenu.prefab", typeof(GameObject));
        if (prefab)
        {
            GameObject newGameObject = GameObject.Instantiate(prefab);
            newGameObject.name = "PauseMenu";
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Pause Menu Prefab!");
        }

        CreateEventSystem();
    }
    
    public static void CreateEventSystem()
    {
        GameObject eventSystemGo = new GameObject("EventSystem", typeof(EventSystem));
        eventSystemGo.AddComponent<StandaloneInputModule>();
        eventSystemGo.transform.position = Vector3.zero;
        Selection.activeGameObject = eventSystemGo;
    }
}
