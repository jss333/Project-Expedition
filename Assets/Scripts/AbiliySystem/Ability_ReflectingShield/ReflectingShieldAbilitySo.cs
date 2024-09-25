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

        public override void UseAbility(Transform spawnPoint)
        {
            _duration = 0;

            newShield = Instantiate(reflectingShieldProperties.abilityPrefab, spawnPoint);

            reflectingShieldController = newShield.GetComponent<ReflectingShieldController>();

            if(reflectingShieldController != null )
            {
                reflectingShieldController.SetReflectionShieldProperties(reflectingShieldProperties);

                AddListenerToEvent(reflectingShieldController.RemoveAbilityVisualFromScene);
            }



            //Debug.Log("Use Reflect Ability");
        }

        public override void UpdateCoolDownTime()
        {
            _coolDownTime += Time.deltaTime;

            if (_coolDownTime > MaxCoolDownTime)
                _coolDownTime = MaxCoolDownTime;
            //Debug.Log("Updating CoolDown " + (int)_coolDownTime);
        }

        public override void UpdateDuration()
        {
            _duration += Time.deltaTime;

            if (_duration >= MaxDuration)
            {
                _duration = MaxDuration;
                _coolDownTime = 0;

                OnAbilityUsedUp?.Invoke();

                RemoveListenerFromEvent(reflectingShieldController.RemoveAbilityVisualFromScene);
            }

            //Debug.Log("Updating Duration " + (int)_duration);
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