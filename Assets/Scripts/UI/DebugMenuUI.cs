using System.Collections;
using System.Collections.Generic;
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
        Time.timeScale = 1;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            isDebugOn = !isDebugOn;
            if(isDebugOn)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
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
