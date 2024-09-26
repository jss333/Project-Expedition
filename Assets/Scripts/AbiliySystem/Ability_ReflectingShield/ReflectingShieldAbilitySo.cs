using System;
using System.Collections;
using System.Collections.Generic;
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

        public override void UseAbility(Transform spawnPoint)
        {
            duration = 0;

            newShield = Instantiate(reflectingShieldProperties.abilityPrefab, spawnPoint);

            reflectingShieldController = newShield.GetComponent<ReflectingShieldController>();

            if(reflectingShieldController != null )
            {
                reflectingShieldController.SetReflectionShieldProperties(reflectingShieldProperties);

                AddListenerToEvent(reflectingShieldController.RemoveAbilityVisualFromScene);

                return;
            }



            reversingDirectionShieldController = newShield.GetComponent<ReversingDirectionShieldController>();

            if(reversingDirectionShieldController != null )
            {
                reversingDirectionShieldController.SetReflectionShieldProperties(reflectingShieldProperties);

                AddListenerToEvent(reversingDirectionShieldController.RemoveAbilityVisualFromScene);
            }

            //Debug.Log("Use Reflect Ability");
        }

        public override void UpdateCoolDownTime()
        {
            coolDownTime += Time.deltaTime;

            if (coolDownTime > MaxCoolDownTime)
                coolDownTime = MaxCoolDownTime;
            //Debug.Log("Updating CoolDown " + (int)_coolDownTime);
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
            }

            //Debug.Log("Updating Duration " + (int)_duration);
        }

        public override void ForceCancelAbility()
        {
            if (reflectingShieldController)
                RemoveListenerFromEvent(reflectingShieldController.RemoveAbilityVisualFromScene);

            if (reversingDirectionShieldController)
                RemoveListenerFromEvent(reversingDirectionShieldController.RemoveAbilityVisualFromScene);
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