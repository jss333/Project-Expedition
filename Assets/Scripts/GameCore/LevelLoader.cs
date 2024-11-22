using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LevelLoader : MonoBehaviour
{

    [SerializeField]
    private new Camera camera; // make this hiden in code but usable in editor inspector

    private Scene MasterScene;
    private bool IsLoading = false;
    // Start is called before the first frame update
    void Start()
    {
        this.MasterScene = gameObject.scene;

        if(!AssignCamera())
        {
            this.IsLoading = true; //block access to functionality and quit.
            return;
        }

        camera.enabled = true;
        Invoke("LoadBossLevel", 2);
    }

    private void LoadBossLevel()
    {
        LoadLevel("Boss01");
    }
    private void LoadLevel(string Level)
    {
        this.IsLoading = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(Level, LoadSceneMode.Additive);
        Time.timeScale = 0.0f; //stop updates
        camera.Render();
    }

    //handle after sene is loaded to handle clean up of actors
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("loading: " + scene.name);
        SceneManager.SetActiveScene(scene);
        Time.timeScale = 1; //re unpause to stop issues.
        camera.enabled = false;
        this.IsLoading = false;
    }
    private void OnSceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        Debug.Log(scene.name + " unloading");
        Time.timeScale = 1.0f;
        SceneManager.SetActiveScene(this.MasterScene);
        Invoke("LoadBossLevel", 2);
    }

    private bool AssignCamera()
    {
        if (!camera)
        {

            camera = GetComponent<Camera>();
            if (!camera)
            {
                camera = GetComponentInChildren<Camera>();
                if (!camera)
                {
                    Debug.LogError("Camera Must be assigned to level tranisiton manager");
                    return false;
                }
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        //// Debug.Log("hello");
        //GameObject[] GameObjects = SceneManager.GetSceneAt(0).GetRootGameObjects();
        //foreach (GameObject gameObject in GameObjects)
        //{
        //    Debug.Log(gameObject.name);
        //}
    }
    public void UnloadLevel()
    {
        if (this.IsLoading) return;
        Time.timeScale = 0.0f;
        this.IsLoading = true;
        camera.enabled = true;

        SceneManager.UnloadSceneAsync("Boss01");
        SceneManager.sceneUnloaded += OnSceneUnloaded;

    }
}
