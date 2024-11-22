using AbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAbilityContainerHandler : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private Transform abilityButtonsContainer;
    [SerializeField] private AbilityDebugButton abilityDebugButtonPrefab;


    void Start()
    {
        if (abilityButtonsContainer.childCount > 0)
        {
            for (int i = 0; i < abilityButtonsContainer.childCount; i++)
            {
                Destroy(abilityButtonsContainer.GetChild(i).gameObject);
            }
        }

        foreach (AbilitySo ability in AbilityManager.singleton.AvailableAbilities)
        {
            AbilityDebugButton abilityDebugButton = Instantiate(abilityDebugButtonPrefab, abilityButtonsContainer);
            if(abilityDebugButton != null)
            {
                abilityDebugButton.Initialize(abilityDebugButton.transform.GetSiblingIndex());
            }
        } 
    }
}
