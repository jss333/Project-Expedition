using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "New BombAbility", menuName = "Ability System/BombAbility")]
    public class BombAbilitySO : AbilitySo
    {
        [Header("Bomb Settings")]
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private float durationInAir;
        [SerializeField] private int explosionDamage = 300;
        [SerializeField] private float explosionRadius = 1.5f;
        public static Action OnAbilityUsedUp;

        public override void UseAbility(Transform spawnPoint)
        {
            duration = 0;
            BombStickyController bombStickyController = Instantiate(bombPrefab, spawnPoint).GetComponent<BombStickyController>();

            if(bombStickyController != null)
            {
                bombStickyController.InitializeBombSettings(durationInAir, explosionDamage, explosionRadius);
            }
        }

        public override void ForceCancelAbility()
        {
        }

        public override void UpdateCoolDownTime()
        {
            coolDownTime += Time.deltaTime;

            if (coolDownTime > MaxCoolDownTime)
            {
                coolDownTime = MaxCoolDownTime;
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
            }
        }
    }
}