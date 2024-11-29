using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBGM : MonoBehaviour
{
    private static MainMenuBGM instance;

    void Awake()
    {
        // Check if an instance of MainMenuBGM already exists
        if (instance == null)
        {
            // If not, set this as the instance and don't destroy it on load
            instance = this;
            //DontDestroyOnLoad(instance);
        }
        else
        {
            // If an instance already exists, destroy this one to avoid duplicates
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        AudioManagerNoMixers.Singleton.PlayMainMenuMusic();
    }
}
