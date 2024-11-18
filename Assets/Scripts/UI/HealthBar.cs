using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    private HealthComponent healthComponent;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
    }
    private void Start()
    {
        SetMaxHealth();
    }
    public void SetMaxHealth()
    {
        if (healthComponent != null)
        {
            slider.maxValue = healthComponent.getMaxHealth();
            slider.value = healthComponent.getCurrentHealth();
        }
    }
    public void SetHealth()
    {
        if (healthComponent != null)
        {
            slider.value = healthComponent.getCurrentHealth();
        }
    }

}