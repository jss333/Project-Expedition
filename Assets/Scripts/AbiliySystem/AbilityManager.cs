using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager singleton;

        [SerializeField] private List<AbilitySo> availableAbilities = new List<AbilitySo>();

        public List<AbilitySo> AvailableAbilities => availableAbilities;
        public Action<int> OnCurrentAbilitySelected;

        [SerializeField] private AbilitySo currentAbility;
        private int currentAbilityIndex = 0;
        private GameObject playerTransform;

        private void Awake()
        {
            if(singleton != null)
            {
                Destroy(this);
            }
            else
            {
                singleton = this;
            }

            
        }

        private void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player");

            foreach(var ability in availableAbilities)
            {
                ability.InitializeAbility();
            }

            SelectFirstAbilityAsCurrent();

            InputHandler.Singleton.OnAbilityActivate += UseCurrentAbility;
            InputHandler.Singleton.OnCycleForward += CycleForwardThroughAbilities;
            InputHandler.Singleton.OnCycleBackward += CycleBackwardThroughAbilities;
        }

        private void OnDestroy()
        {
            InputHandler.Singleton.OnAbilityActivate -= UseCurrentAbility;
            InputHandler.Singleton.OnCycleForward -= CycleForwardThroughAbilities;
            InputHandler.Singleton.OnCycleBackward -= CycleBackwardThroughAbilities;
        }


        private void SelectFirstAbilityAsCurrent()
        {
            currentAbility = availableAbilities[currentAbilityIndex];
            OnCurrentAbilitySelected?.Invoke(currentAbilityIndex);
        }

        private void Update()
        {
            UpdateDurations();
            UpdateCoolDowns();
        }
        private void UpdateDurations()
        {
            foreach (var ability in availableAbilities)
            {
                if (ability.InUse())
                {
                    ability.UpdateDuration();
                }
            }
        }

        private void UpdateCoolDowns()
        {
            foreach (var ability in availableAbilities)
            {
                if (ability.InCoolDown())
                {
                    ability.UpdateCoolDownTime();
                }
            }
        }


        private void UseCurrentAbility()
        {
            if (currentAbility == null || currentAbility.InUse())
            {
                return;
            }

            if (currentAbility.ReadyToUse())
            {
                currentAbility.UseAbility(playerTransform.transform);
            }
        }

        public void CycleForwardThroughAbilities()
        {
            currentAbilityIndex++;
            if(currentAbilityIndex >= availableAbilities.Count)
            {
                currentAbilityIndex = 0;
            }

            currentAbility = availableAbilities[currentAbilityIndex];
            OnCurrentAbilitySelected?.Invoke(currentAbilityIndex);
        }

        public void CycleBackwardThroughAbilities()
        {
            currentAbilityIndex--;
            if (currentAbilityIndex < 0)
            {
                currentAbilityIndex = availableAbilities.Count - 1;
            }

            currentAbility = availableAbilities[currentAbilityIndex];
            OnCurrentAbilitySelected?.Invoke(currentAbilityIndex);
        }


        public void AddAbilityToInventory(AbilitySo ability)
        {
            availableAbilities.Add(ability);

            if (availableAbilities.Count == 1)
            {
                SelectFirstAbilityAsCurrent();
            }
        }

        public void RemoveAbilityFromInventory(AbilitySo ability)
        {
            // TODO consider what happens if ability removed is selected?
            availableAbilities.Remove(ability);
        }
        public AbilitySo CheckForRunningAbility()
        {
            foreach(var ability in availableAbilities) 
            {
                if (ability.InUse() && ability != currentAbility)
                {
                    return ability;
                }
            }
            return null;
        }
    }

    public enum AbilityType { ReflectingShield, TestAbility}

    [System.Serializable]
    public struct ReflectingShieldProperties
    {
        public AbilityType abilityType;
        public GameObject abilityPrefab;
        public float radius;
        public float reverseSpeed;
        public AudioClip chargeClip;
        public bool shouldDestroyProjectileOnImpact;

        [Range(5, 300)]
        public int newDamageValue;
    }
}