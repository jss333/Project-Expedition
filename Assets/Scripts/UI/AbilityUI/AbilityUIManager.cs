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

        private void OnDestroy()
        {
            AbilityManager.singleton.OnCurrentAbilitySelected -= HighLightCurrentlySelectedAbility;
        }

        void Update()
        {
            UpdateAbilityUIItemVisuals();
        }

        private void InitializeAbilitiesUI()
        {
            List<AbilitySo> abilities = AbilityManager.singleton.AvailableAbilities;

            abilityUIHolder.DetachChildren();

            if (abilities.Count > 0)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    AbilityUIItem abilityUIItem = Instantiate(abilityUIItemPrefab.gameObject, abilityUIHolder).GetComponent<AbilityUIItem>();

                    abilityUIItems.Add(abilityUIItem);

                    if (abilityUIItem != null )
                    {
                        abilityUIItem.InitializeUI(AbilityManager.singleton.AvailableAbilities[i], i);
                    }
                }
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

        private void UpdateAbilityUIItemVisuals()
        {
            for (int i = 0; i < abilityUIItems.Count; i++)
            {
                abilityUIItems[i].UpdateVisual(AbilityManager.singleton.AvailableAbilities[i]);
            }
        }
    }
}