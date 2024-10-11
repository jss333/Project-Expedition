using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorMenus : EditorWindow
{
    [MenuItem("Level Tools/Core Objects/Set Up Level Objects", false, 0)]
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
        CreateHorizontalPlatform();
    }

    [MenuItem("Level Tools/Core Objects/Create Audio Manager")]
    public static void CreateAudioManager()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/Audio/AudioManager.prefab");
    }

    [MenuItem("Level Tools/Core Objects/Create Player")]
    public static void CreatePlayer()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/Player/R0B-B3RT.prefab");
    }

    [MenuItem("Level Tools/Core Objects/Create Ability Manager")]
    public static void CreateAbilityManager()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/AbilitySystem/AbilityManager.prefab");
    }

    [MenuItem("Level Tools/Core Objects/Create Input Handler")]
    public static void CreateInputHandler()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/InputHandler/InputHandler.prefab");
    }

    [MenuItem("Level Tools/Core Objects/Create Boss")]
    public static void CreateBoss()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/Enemy/Boss/Boss.prefab");
    }


    [MenuItem("Level Tools/Core Objects/Create Camera System")]
    public static void CreateCameraSystem()
    {
        CreateCameraConfine();
        CreateCameraObject();
        CreateVirtualCamera();
    }

    private static void CreateCameraConfine()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/Camera/CameraConfine.prefab");
    }

    private static void CreateCameraObject()
    {
        GameObject oldCamera = GameObject.FindObjectOfType<Camera>().gameObject;
        if(oldCamera != null)
        {
            DestroyImmediate(oldCamera);
        }

        LoadAssetPrefabFromPath("Assets/Prefabs/Camera/Main Camera.prefab");
    }

    private static void CreateVirtualCamera()
    {
        GameObject virtualCameraPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Camera/Virtual Camera.prefab", typeof(GameObject));
        if (virtualCameraPrefab)
        {
            CreateAndConfigureVirtualCamera(virtualCameraPrefab);
        }
        else
        {
            EditorUtils.DisplayDialogBox("Unable to Find the Virtual Camera Prefab!");
        }
    }

    private static void CreateAndConfigureVirtualCamera(GameObject virtualCameraPrefab)
    {
        GameObject virtualCamera = (GameObject)PrefabUtility.InstantiatePrefab(virtualCameraPrefab);

        CinemachineVirtualCamera cinemachineVirtualCamera = virtualCamera.GetComponent<CinemachineVirtualCamera>();
        if (cinemachineVirtualCamera != null)
        {
            Transform playerTransform = GameObject.Find("R0B").transform;
            cinemachineVirtualCamera.Follow = playerTransform;
            cinemachineVirtualCamera.LookAt = playerTransform;
        }

        CinemachineConfiner cinemachineConfiner = virtualCamera.GetComponent<CinemachineConfiner>();
        if (cinemachineConfiner != null)
        {
            cinemachineConfiner.m_BoundingShape2D = FindObjectOfType<PolygonCollider2D>();
        }
    }

    [MenuItem("Level Tools/Core Objects/Debug Menu")]
    public static void CreateDebugMenu()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/UI/DebugMenu.prefab");

        CreateEventSystemIfNotAlreadyPresent();
    }

    [MenuItem("Level Tools/Core Objects/Pause Menu")]
    public static void CreatePauseMenu()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/UI/PauseMenu.prefab");

        CreateEventSystemIfNotAlreadyPresent();
    }

    private static void CreateEventSystemIfNotAlreadyPresent()
    {
        if (GameObject.FindObjectOfType<EventSystem>().gameObject == null)
        {
            LoadAssetPrefabFromPath("Assets/Prefabs/UI/EventSystem.prefab");
        }
    }

    public static void CreateHealthBars()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/UI/HealthBar.prefab");
    }


    [MenuItem("Level Tools/Core Objects/Create Level Manager")]
    public static void CreateLevelManager()
    {
        LoadAssetPrefabFromPath("Assets/Prefabs/LevelsManagement/LevelManager.prefab");
    }

    [MenuItem("Level Tools/Core Objects/Create Platform - Horizontal")]
    public static void CreateHorizontalPlatform()
    {
        GameObject newPlatform = LoadAssetPrefabFromPath("Assets/Prefabs/Platforms/Horizontal Platform.prefab");
        if(newPlatform != null )
        {
            Transform playerTransform = GameObject.Find("R0B-B3RT").transform;
            float x = playerTransform ? playerTransform.position.x : 0;
            newPlatform.transform.position = new Vector3(x, 0, 0);
        }
    }

    [MenuItem("Level Tools/Core Objects/Create Platform - Vertical")]
    public static void CreateVerticalPlatform()
    {
        GameObject newPlatform = LoadAssetPrefabFromPath("Assets/Prefabs/Platforms/Vertical Platform.prefab");
        if (newPlatform != null)
        {
            newPlatform.transform.position = new Vector3(0, 0, 0);
        }
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
            EditorUtils.DisplayDialogBox("Unable to Find Prefab at " + path + "!");
            return null;
        }
    }
}
