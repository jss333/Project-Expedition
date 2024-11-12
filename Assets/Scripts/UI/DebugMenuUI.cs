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
    [Header("AbilityVariables")]
    public List<AbilityButton> abilityButtons = new List<AbilityButton>();
    [Header("References")]
    [SerializeField] private Sprite greenButton;
    [SerializeField] private Sprite redButton;

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

    public void LevelChange(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void toggleAbilityImage(string ability)
    {
        for (int i = 0; i < abilityButtons.Count; i++)
        {
            if(ability == abilityButtons[i].abilityName)
            {
                if (abilityButtons[i].isActive)
                {
                    abilityButtons[i].image.sprite = redButton;
                    abilityButtons[i].isActive = false;
                    //Debug.Log(abilityButtons[i].abilityName +" "+ abilityButtons[i].isActive);
                }
                else
                {
                    abilityButtons[i].image.sprite = greenButton;
                    abilityButtons[i].isActive = true;
                    //Debug.Log(abilityButtons[i].abilityName + " " + abilityButtons[i].isActive);
                }
                
            }
        }
    }
}
