using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager Singleton;

        [SerializeField] private List<AbilitySo> availableAbilities = new List<AbilitySo>();
        [SerializeField] private AbilitySo currentAbility;
        private AbilityToggleUI toggleUI;

        int currentAbilityIndex = 0;

        GameObject playerTransform;


        public List<AbilitySo> AvailableAbilities => availableAbilities;


        public Action<int> OnCurrentAbilitySelected;

        private void Awake()
        {
            if(Singleton != null)
            {
                Destroy(Singleton);
            }
            else
            {
                Singleton = this;
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
        }

        public void AddAbilityToInventory(AbilitySo _ability)
        {
            availableAbilities.Add(_ability);

            SelectFirstAbilityAsCurrent();
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

            availableAbilities[1].InitializeAbility();
        }

        private void UseCurrentAbility()
        {
            if(currentAbility == null) return;

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

            Debug.Log(currentAbilityIndex);

            OnCurrentAbilitySelected?.Invoke(currentAbilityIndex);

            currentAbility = availableAbilities[currentAbilityIndex];
            //currentAbility.InitializeAbility();
        }

        public void CycleBackwardThroughAbilities()
        {
            currentAbilityIndex--;

            if (currentAbilityIndex < 0)
            {
                currentAbilityIndex = availableAbilities.Count - 1;
            }

            Debug.Log(currentAbilityIndex);

            OnCurrentAbilitySelected?.Invoke(currentAbilityIndex);

            currentAbility = availableAbilities[currentAbilityIndex];
            //currentAbility.InitializeAbility();
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