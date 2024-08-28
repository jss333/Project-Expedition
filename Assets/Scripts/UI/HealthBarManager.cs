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

    // Start is called before the first frame update
    void Start()
    {
        playerController = new PlayerController();        
        playerController.health.currentHP = 1;
        UpdateHealth();
        Debug.Log("player controller health: " + playerController.health);
    }

    // Update is called once per frame
    public void UpdateHealth()
    {          
        float healthRatio = maxBarLength * (playerController.health.currentHP / playerController.health.maxHP);
        // not working yet
        healthBar.transform.localScale = new Vector3(healthRatio, 1, 1);        
    }
}
