using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesGetter : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private Transform sceneButtonsContainer;
    [SerializeField] private LoadLevelButton loadLevelButtonPrefab;
    private List<string> Scenes = new List<string>();


    void Start()
    {
        if(sceneButtonsContainer.childCount > 0)
        {
            for(int i = 0; i < sceneButtonsContainer.childCount; i++)
            {
                Destroy(sceneButtonsContainer.GetChild(i).gameObject);
            }
        }

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);

            LoadLevelButton loadLevelButton = Instantiate(loadLevelButtonPrefab, sceneButtonsContainer);
            if(loadLevelButton != null)
            {
                loadLevelButton.Initialize(sceneName);
            }
        } 
    }
}
