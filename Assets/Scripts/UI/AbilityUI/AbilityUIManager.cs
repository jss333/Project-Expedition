using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityUIManager : MonoBehaviour
    {
        [SerializeField] private Transform abilityUIHolder;
        [SerializeField] private AbilityUIItem abilityUIItemPrefab;

        private List<AbilitySo> abilities = new List<AbilitySo>();
        private List<AbilityUIItem> abilityUIItems = new List<AbilityUIItem>();

        private void Start()
        {
            abilities = AbilityManager.Singleton.AvailableAbilities;

            AbilityManager.Singleton.OnCurrentAbilitySelected += HighLightCurrentlySelectedAbility;

            InitializeAbilitiesUI();
        }

        private void OnDestroy()
        {
            AbilityManager.Singleton.OnCurrentAbilitySelected -= HighLightCurrentlySelectedAbility;
        }

        void Update()
        {
            UpdateAbilityUIItemVisuals();
        }

        private void InitializeAbilitiesUI()
        {
            abilityUIHolder.DetachChildren();

            if (abilities.Count > 0)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    AbilityUIItem abilityUIItem = Instantiate(abilityUIItemPrefab.gameObject, abilityUIHolder).GetComponent<AbilityUIItem>();

                    abilityUIItems.Add(abilityUIItem);

                    if (abilityUIItem != null )
                    {
                        abilityUIItem.InitializeUI(abilities[i], i);
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
                abilityUIItems[i].UpdateVisual(abilities[i]);
            }
        }
    }
}