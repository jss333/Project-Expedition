using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager Singletone;

        [SerializeField] List<AbilitySo> availableAbilities = new List<AbilitySo>();
        [SerializeField] AbilitySo firstAbility;
        [SerializeField] AbilitySo secondaryAbility;
        [SerializeField] AbilityToggleUI toggleUI;


        private bool toggleSecondAb = false;
        private bool toggleFirstAb = false;
        int currentAbilityIndex = 0;

        GameObject playerTransform;

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

            InputHandler.Singletone.OnAbilityActivate += UseFirstAbility;
            InputHandler.Singletone.OnSecondaryAbilityActivate += UseSecondaryAbility;
        }

        private void OnDestroy()
        {
            InputHandler.Singletone.OnAbilityActivate -= UseFirstAbility;
            InputHandler.Singletone.OnSecondaryAbilityActivate -= UseSecondaryAbility;
        }

        private void Start()
        {
            toggleUI = FindFirstObjectByType<AbilityToggleUI>();
            playerTransform = GameObject.FindGameObjectWithTag("Player");
            firstAbility.InitializeAbility();
            secondaryAbility.InitializeAbility();
        }

        private void Update()
        {
            if(firstAbility.InUse())
                firstAbility.UpdateDuration();

            if(secondaryAbility.InUse())
                secondaryAbility.UpdateDuration();

            if (firstAbility.IsInCoolDown())
                firstAbility.UpdateCoolDownTime();
            
            if (secondaryAbility.IsInCoolDown())
                secondaryAbility.UpdateCoolDownTime();
            toggleFirstAb = toggleUI.ShieldAbilityBlue;
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

        [ContextMenu("Use Current Ability")]
        private void UseFirstAbility()
        {
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
            if (secondaryAbility != null && toggleSecondAb)
            {
                if (firstAbility.InUse())
                    return;

                if (secondaryAbility.IsReadyToUse())
                    secondaryAbility.UseAbility(playerTransform.transform);
            }
        }

        [ContextMenu("Cycle")]
        public void CycleThroughAbilities()
        {
            if(firstAbility != null)
            {
                if (firstAbility.InUse()) return;
            }

            currentAbilityIndex++;

            if(currentAbilityIndex >= availableAbilities.Count)
            {
                currentAbilityIndex = 0;
            }

            if (currentAbilityIndex < 0)
            {
                currentAbilityIndex = availableAbilities.Count - 1;
            }

            firstAbility = availableAbilities[currentAbilityIndex];
            firstAbility.InitializeAbility();
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


        [Range(5, 300)]
        public int newDamageValue;
    }

    [System.Serializable]
    public struct TestAbilityProperties
    {
        public string message;
    }
}