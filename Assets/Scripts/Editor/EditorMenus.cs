using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorMenus : EditorWindow
{
    [MenuItem("GameObject/Core Objects/Setup Level Objects", false, 0)]
    [MenuItem("LevelTools/Core Objects/Setup Level Objects", false, 0)]
    public static void CreateAllObjects()
    {
        CreatePlayer();
        CreateCameraSystem();
        //CreatePauseMenu();
        CreateDebugMenu();
        //CreateBoss();
        CreateAudioManager();
        CreateAbilityManager();
        CreateInputHandler();
        CreateHealthBars();
        CreateLevelManager();
        CreatePlatform();
    }




    [MenuItem("LevelTools/Core Objects/Create Audio Manager")]
    public static void CreateAudioManager()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/Audio/Challenge Room BGM.prefab");
    }

    [MenuItem("LevelTools/Core Objects/Create Player")]
    public static void CreatePlayer()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/Player/R0B-B3RT.prefab");
    }

    [MenuItem("LevelTools/Core Objects/Create Ability Manager")]
    public static void CreateAbilityManager()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/AbilitySystem/AbilityManager.prefab");
    }

    [MenuItem("LevelTools/Core Objects/Create Input Handler")]
    public static void CreateInputHandler()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/InputHandler/InputHandler.prefab");
    }

    [MenuItem("LevelTools/Core Objects/Create Boss")]
    public static void CreateBoss()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/Enemy/Boss/Boss.prefab");
    }

    [MenuItem("LevelTools/Core Objects/Create Camera System")]
    public static void CreateCameraSystem()
    {
        CreateCameraConfine();
        CreateCameraObject();
        CreateVirtualCamera();
    }

    public static void CreateCameraObject()
    {
        GameObject oldCamera = GameObject.FindObjectOfType<Camera>().gameObject;
        if(oldCamera != null) 
            DestroyImmediate(oldCamera);

        LoadAssetPrefabFromPath("Assets/Prefabs/Camera/Main Camera.prefab");
    }

    public static void CreateVirtualCamera()
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Camera/Virtual Camera.prefab", typeof(GameObject));
        if (prefab)
        {
            GameObject newGameObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            CinemachineVirtualCamera cinemachineVirtualCamera = newGameObject.GetComponent<CinemachineVirtualCamera>();
            if(cinemachineVirtualCamera != null)
            {
                Transform playerTransform = GameObject.Find("R0B-B3RT").transform.GetChild(0);
                cinemachineVirtualCamera.Follow = playerTransform;
                cinemachineVirtualCamera.LookAt = playerTransform;
            }


            CinemachineConfiner cinemachineConfiner = newGameObject.GetComponent<CinemachineConfiner>();
            if(cinemachineConfiner != null )
            {
                cinemachineConfiner.m_BoundingShape2D = FindObjectOfType<PolygonCollider2D>();
            }
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Virtual Camera Prefab!");
        }
    }  
    
    public static void CreateCameraConfine()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/Camera/CameraConfine.prefab");
    }


    [MenuItem("LevelTools/Core Objects/Debug Menu")]
    public static void CreateDebugMenu()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/UI/DebugMenu.prefab");
    }

    [MenuItem("LevelTools/Core Objects/Pause Menu")]
    public static void CreatePauseMenu()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/UI/PauseMenu.prefab");

        CreateEventSystem();
    }

    public static void CreateHealthBars()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/UI/HealthBar.prefab");

        CreateEventSystem();
    }

    [MenuItem("LevelTools/Core Objects/Create Level Manager")]
    public static void CreateLevelManager()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/LevelsManagement/LevelManager.prefab");
    }

    [MenuItem("LevelTools/Core Objects/Create A Test Platform")]
    public static void CreatePlatform()
    {
        GameObject newPlatform = LoadAssetPrefabFromPath("Assets/Prefabs/Platforms/Horizontal Platform.prefab");
        if(newPlatform != null )
        {
            Transform playerTransform = GameObject.Find("R0B-B3RT").transform;

            Vector3 platformPos = new Vector3(playerTransform.position.x, 0, 0);
            newPlatform.transform.position = platformPos;
        }
    }

    public static void CreateEventSystem()
    {
        GameObject eventSystemGo = new GameObject("EventSystem", typeof(EventSystem));
        eventSystemGo.AddComponent<StandaloneInputModule>();
        eventSystemGo.transform.position = Vector3.zero;
        Selection.activeGameObject = eventSystemGo;
    }

    private static GameObject LoadAssetPrefabFromPath(string path)
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        if (prefab)
        {
            return (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find A Prefab!");
            return null;
        }
    }
}
