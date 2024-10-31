using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "New ReflectingShieldAbility", menuName = "Ability System/ReflectingShieldAbility")]
    public class ReflectingShieldAbilitySo : AbilitySo
    {
        [Header("Custom Settings")]
        public ReflectingShieldProperties reflectingShieldProperties;
        public static Action OnAbilityUsedUp;

        private GameObject newShield;
        ReflectingShieldController reflectingShieldController;
        ReversingDirectionShieldController reversingDirectionShieldController;
        RemovingProjectileShieldController removingProjectileShieldController;
        AbilitySo ability;

        public override void UseAbility(Transform spawnPoint)
        {
            duration = 0;
            newShield = Instantiate(reflectingShieldProperties.abilityPrefab, spawnPoint);

            reflectingShieldController = newShield.GetComponent<ReflectingShieldController>();
            if(reflectingShieldController != null )
            {
                Debug.Log("reflecting shield");
                ability = AbilityManager.singleton.CheckForRunningAbility();
                if (ability != null)
                {
                    Debug.Log("ability in use");
                    ability.InterruptAbility();
                }
                reflectingShieldController.SetReflectionShieldProperties(reflectingShieldProperties);
                AddListenerToEvent(reflectingShieldController.RemoveAbilityVisualFromScene);
                return;
            }
            
            reversingDirectionShieldController = newShield.GetComponent<ReversingDirectionShieldController>();
            if(reversingDirectionShieldController != null )
            {
                Debug.Log("reverse direction");
                reversingDirectionShieldController.SetReflectionShieldProperties(reflectingShieldProperties);
                AddListenerToEvent(reversingDirectionShieldController.RemoveAbilityVisualFromScene);
            }
            
            removingProjectileShieldController = newShield.GetComponent<RemovingProjectileShieldController>();
            if (removingProjectileShieldController != null)
            {
                Debug.Log("removing shield");
                removingProjectileShieldController.SetReflectionShieldProperties(reflectingShieldProperties);
                AddListenerToEvent(removingProjectileShieldController.RemoveAbilityVisualFromScene);
            }
        }

        public override void UpdateCoolDownTime()
        {
            coolDownTime += Time.deltaTime;

            if (coolDownTime > MaxCoolDownTime)
            {
                coolDownTime = MaxCoolDownTime;
                ReCharge();
            }
        }

        public override void UpdateDuration()
        {
            duration += Time.deltaTime;

            if (duration >= MaxDuration)
            {
                duration = MaxDuration;
                coolDownTime = 0;

                OnAbilityUsedUp?.Invoke();

                if(reflectingShieldController)
                    RemoveListenerFromEvent(reflectingShieldController.RemoveAbilityVisualFromScene);

                if (reversingDirectionShieldController)
                    RemoveListenerFromEvent(reversingDirectionShieldController.RemoveAbilityVisualFromScene);

                if (removingProjectileShieldController)
                    RemoveListenerFromEvent(removingProjectileShieldController.RemoveAbilityVisualFromScene);
            }
        }

        public override void ForceCancelAbility()
        {
            if (reflectingShieldController)
            {
                RemoveListenerFromEvent(reflectingShieldController.RemoveAbilityVisualFromScene);
            }
            if (reversingDirectionShieldController)
            {
                RemoveListenerFromEvent(reversingDirectionShieldController.RemoveAbilityVisualFromScene);
            } 
            if (removingProjectileShieldController)
            {
                RemoveListenerFromEvent(removingProjectileShieldController.RemoveAbilityVisualFromScene);
            }
                
        }
        private void ReCharge()
        {
            if (reflectingShieldController)
                reflectingShieldController.OnRechargeAbility();

            if (reversingDirectionShieldController)
                reversingDirectionShieldController.OnRechargeAbility(); 
            
            if (removingProjectileShieldController)
                removingProjectileShieldController.OnRechargeAbility();
        }

        private void RemoveAbilityObjectFromScene()
        {
            if (reflectingShieldController)
                reflectingShieldController.RemoveAbilityGameObjectFromScene();

            if (reversingDirectionShieldController)
                reversingDirectionShieldController.RemoveAbilityGameObjectFromScene();
            
            if (removingProjectileShieldController)
                removingProjectileShieldController.RemoveAbilityGameObjectFromScene();
        }

        private void AddListenerToEvent(Action _listener)
        {
            OnAbilityUsedUp += _listener;
        }

        private void RemoveListenerFromEvent(Action _listener)
        {
            OnAbilityUsedUp -= _listener;
        }
    }
}