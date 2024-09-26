using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Platformer.Core;
using Platformer.Mechanics;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    public PlayerController playerController;
    public GameObject playerObj;
    [SerializeField] float maxBarLength;
    [SerializeField] GameObject victoryScreen; // Reference to the victory screen
    [SerializeField] GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        playerController = new PlayerController();        
        playerController.health.currentHP = 1;
        UpdateHealth();
        Debug.Log("player controller health: " + playerController.health);
        victoryScreen.SetActive(false);
    }

    // Update is called once per frame
    public void UpdateHealth()
    {          
        float healthRatio = maxBarLength * (playerController.health.currentHP / playerController.health.maxHP);
        // not working yet
        healthBar.transform.localScale = new Vector3(healthRatio, 1, 1);
        if (playerController.health.currentHP <= 0)
        {
            ActivateVictoryScreen();
        }
    }

    void ActivateVictoryScreen()
    {
        // Deactivate the pause menu if it's active
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
        victoryScreen.SetActive(true);  // Show the victory screen
        Time.timeScale = 0f; // Pause the game
    }
}
