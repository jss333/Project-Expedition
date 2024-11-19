using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugMenuUI : MonoBehaviour
{
    public GameObject debugMenuUI;
    private bool isDebugOn = false;


    private void Start()
    {
        isDebugOn = false;
        //instance.pauseGame(false);

        InputHandler.Singleton.OnHandleDebugMenuOpenning += HandleDebugMenuOpenning;
    }

    private void OnDestroy()
    {
        InputHandler.Singleton.OnHandleDebugMenuOpenning -= HandleDebugMenuOpenning;
    }

    void Update()
    {
        // Toggle pause when the "P" key is pressed
        if (isDebugOn)
        {
            debugMenuUI.SetActive(true);
        }
        else
        {
            debugMenuUI.SetActive(false);
        }
    }

    private void HandleDebugMenuOpenning()
    {
        isDebugOn = !isDebugOn;
        if (isDebugOn)
        {
            InputHandler.Singleton.OnUIMenuActivated?.Invoke();
            PauseManager.Singletone.pauseGame(true);
        }
        else
        {
            InputHandler.Singleton.OnUIMenuDeActivated?.Invoke();
            PauseManager.Singletone.pauseGame(false);
        }
    }
}
