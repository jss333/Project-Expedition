using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityUIManager : MonoBehaviour
    {
        [SerializeField] private Transform abilityUIHolder;
        [SerializeField] private AbilityUIItem abilityUIItemPrefab;

        private List<AbilityUIItem> abilityUIItems = new List<AbilityUIItem>();

        private void Start()
        {
            AbilityManager.singleton.OnCurrentAbilitySelected += HighLightCurrentlySelectedAbility;

            InitializeAbilitiesUI();
        }

        private void InitializeAbilitiesUI()
        {
            abilityUIHolder.DetachChildren();

            foreach (var ability in AbilityManager.singleton.AvailableAbilities)
            {
                AbilityUIItem abilityUIItem = Instantiate(abilityUIItemPrefab.gameObject, abilityUIHolder).GetComponent<AbilityUIItem>();

                if (abilityUIItem != null)
                {
                    abilityUIItems.Add(abilityUIItem);
                    abilityUIItem.InitializeUI(ability);
                }
            }
        }

        private void OnDestroy()
        {
            AbilityManager.singleton.OnCurrentAbilitySelected -= HighLightCurrentlySelectedAbility;
        }

        void Update()
        {
            UpdateAbilityUIItemVisuals();
        }

        private void UpdateAbilityUIItemVisuals()
        {
            for (int i = 0; i < abilityUIItems.Count; i++)
            {
                abilityUIItems[i].UpdateCooldownVisual(AbilityManager.singleton.AvailableAbilities[i]);
            }
        }

        private void HighLightCurrentlySelectedAbility(int targetIndex)
        {
            if (abilityUIItems.Count > 0)
            {
                for (int i = 0; i < abilityUIItems.Count; i++)
                {
                    abilityUIItems[i].HideHighLight();

                    if (i == targetIndex)
                    {
                        abilityUIItems[i].ShowHighLight();
                    }
                }
            }
        }

        
    }
}