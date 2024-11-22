using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    #region Singleton;
    public static LevelManager Singleton;

    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            if (Singleton != this)
                Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        LoadUIScene();
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    private void LoadUIScene()
    {
        SceneManager.LoadScene("UIAndIndependables", LoadSceneMode.Additive);
    }
}
