using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager Singletone;

        [SerializeField] private List<AbilitySo> availableAbilities = new List<AbilitySo>();
        [SerializeField] private AbilitySo currentAbility;
        [SerializeField] private AbilitySo firstAbility;
        [SerializeField] private AbilitySo secondaryAbility;
        private AbilityToggleUI toggleUI;


        private bool toggleSecondAb = true;
        private bool toggleFirstAb = true;
        int currentAbilityIndex = 0;

        GameObject playerTransform;


        public List<AbilitySo> AvailableAbilities => availableAbilities;


        public Action<int> OnCurrentAbilitySelected;

        private void Awake()
        {
            if(Singletone != null)
            {
                Destroy(Singletone);
            }
            else
            {
                Singletone = this;
            }

            InputHandler.Singletone.OnAbilityActivate += UseCurrentAbility;
            InputHandler.Singletone.OnCycleForward += CycleForwardThroughAbilities;
            InputHandler.Singletone.OnCycleBackward += CycleBackwardThroughAbilities;
        }

        private void OnDestroy()
        {
            InputHandler.Singletone.OnAbilityActivate -= UseCurrentAbility;
            InputHandler.Singletone.OnCycleForward -= CycleForwardThroughAbilities;
            InputHandler.Singletone.OnCycleBackward -= CycleBackwardThroughAbilities;
        }

        private void Start()
        {
            toggleUI = FindFirstObjectByType<AbilityToggleUI>();
            playerTransform = GameObject.FindGameObjectWithTag("Player");
            /*firstAbility.InitializeAbility();
            secondaryAbility.InitializeAbility(); */

            SelectFirstAbilityAsCurrent();
        }

        private void Update()
        {
            UpdateDurations();

            UpdateCoolDowns();

            /*if(firstAbility.InUse())
                firstAbility.UpdateDuration();

            if(secondaryAbility.InUse())
                secondaryAbility.UpdateDuration();

            if (firstAbility.IsInCoolDown())
                firstAbility.UpdateCoolDownTime();
            
            if (secondaryAbility.IsInCoolDown())
                secondaryAbility.UpdateCoolDownTime(); */

            toggleSecondAb = toggleUI.ShieldAbilityRed;
        }

        public void AddAbilityToInventory(AbilitySo _ability)
        {
            availableAbilities.Add(_ability);
        }

        public void RemoveAbilityFromInventory(AbilitySo _ability)
        {
            availableAbilities.Remove(_ability);
        }

        private void SelectFirstAbilityAsCurrent()
        {
            currentAbility = availableAbilities[0];

            OnCurrentAbilitySelected?.Invoke(0);

            currentAbility.InitializeAbility();
        }

        private void UseCurrentAbility()
        {
            if (currentAbility.InUse())
                return;

            if (currentAbility.IsReadyToUse())
                currentAbility.UseAbility(playerTransform.transform);
        }

        public void CycleForwardThroughAbilities()
        {
            currentAbilityIndex++;

            if(currentAbilityIndex >= availableAbilities.Count)
            {
                currentAbilityIndex = 0;
            }

            OnCurrentAbilitySelected?.Invoke(currentAbilityIndex);

            currentAbility = availableAbilities[currentAbilityIndex];
            currentAbility.InitializeAbility();
        }

        public void CycleBackwardThroughAbilities()
        {
            currentAbilityIndex--;

            if (currentAbilityIndex < 0)
            {
                currentAbilityIndex = availableAbilities.Count - 1;
            }

            OnCurrentAbilitySelected?.Invoke(currentAbilityIndex);

            currentAbility = availableAbilities[currentAbilityIndex];
            currentAbility.InitializeAbility();
        }

        private void UpdateDurations()
        {
            for (int i = 0; i < availableAbilities.Count; i++)
            {
                if (availableAbilities[i].InUse())
                    availableAbilities[i].UpdateDuration();
            }
        }

        private void UpdateCoolDowns()
        {
            for (int i = 0; i < availableAbilities.Count; i++)
            {
                if (availableAbilities[i].IsInCoolDown())
                    availableAbilities[i].UpdateCoolDownTime();
            }
        }

        #region oldAbilityLogic 

        private bool AnyAbilityInUse()
        {
            return firstAbility.InUse() || secondaryAbility.InUse();
        }

        private void UseFirstAbility()
        {

            if (AnyAbilityInUse()) return;

            toggleFirstAb = toggleUI.ShieldAbilityBlue;

            if (firstAbility != null && toggleFirstAb)
            {
                if (secondaryAbility.InUse())
                    return;

                if (firstAbility.IsReadyToUse())
                    firstAbility.UseAbility(playerTransform.transform);
            }
        }

        private void UseSecondaryAbility()
        {

            if (AnyAbilityInUse()) return;

            toggleSecondAb = toggleUI.ShieldAbilityRed;

            if (secondaryAbility != null && toggleSecondAb)
            {
                if (firstAbility.InUse())
                    return;

                if (secondaryAbility.IsReadyToUse())
                    secondaryAbility.UseAbility(playerTransform.transform);
            }
        }
        #endregion
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

    [System.Serializable]
    public struct TestAbilityProperties
    {
        public string message;
    }
}