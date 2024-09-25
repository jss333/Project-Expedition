using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class AbilitySo : ScriptableObject
    {
        [Header("General Settings")]
        [SerializeField] private string _abilityName;
        [SerializeField] private Sprite _abilityIcon;

        [SerializeField] private float _maxDuration;
        protected float _duration;

        [SerializeField] private float _maxCoolDownTime;
        protected float _coolDownTime;

        public float MaxDuration => _maxDuration;
        public float Duration => _duration;

        public float CoolDownTime => _coolDownTime;
        public float MaxCoolDownTime => _maxCoolDownTime;

        public string AbilityName => _abilityName;

        public Sprite AbilityIcon => _abilityIcon;


        private void Awake()
        {
            InitializeAbility();
        }

        public void InitializeAbility()
        {
            _duration = _maxDuration;
            _coolDownTime = _maxCoolDownTime;
        }

        public bool InUse()
        {
            return _duration < _maxDuration;
        }

        public bool IsReadyToUse()
        {
            return (_coolDownTime >= _maxCoolDownTime && _duration >= MaxDuration);
        }

        public bool IsInCoolDown()
        {
            return _coolDownTime < _maxCoolDownTime;
        }

        public abstract void UpdateDuration();

        public abstract void UpdateCoolDownTime();

        public abstract void UseAbility(Transform spawnPoint);
        
    }
}