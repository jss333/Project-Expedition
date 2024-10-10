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

        private void Awake()
        {
            abilities = AbilityManager.Singletone.AvailableAbilities;

            AbilityManager.Singletone.OnCurrentAbilitySelected += HighLightCurrentlySelectedAbility;

            InitializeAbilitiesUI();
        }

        private void OnDestroy()
        {
            AbilityManager.Singletone.OnCurrentAbilitySelected -= HighLightCurrentlySelectedAbility;
        }

        void Update()
        {

        }

        private void InitializeAbilitiesUI()
        {
            if (abilities.Count > 0)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    AbilityUIItem abilityUIItem = Instantiate(abilityUIItemPrefab.gameObject, abilityUIHolder).GetComponent<AbilityUIItem>();

                    abilityUIItems.Add(abilityUIItem);

                    if (abilityUIItem != null )
                    {
                        abilityUIItem.InitializeUI(abilities[i]);
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
    }
}