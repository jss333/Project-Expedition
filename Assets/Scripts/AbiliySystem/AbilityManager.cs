using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager Singletone;

        [SerializeField] List<AbilitySo> _availableAbilities = new List<AbilitySo>();
        [SerializeField] AbilitySo _currentAbility;

        int _currentAbilityIndex = 0;


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


            InputHandler.Singletone.OnAbilityActivate += UseAbility;
        }


        private void OnDestroy()
        {
            InputHandler.Singletone.OnAbilityActivate -= UseAbility;
        }

        private void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player");

            CycleThroughAbilities();
        }


        private void Update()
        {
            //if (_currentAbility == null) return;

            UpdateDurations();

            UpdateCoolDowns();
        }


        public void AddAbilityToInventory(AbilitySo _ability)
        {
            _availableAbilities.Add(_ability);
        }

        public void RemoveAbilityFromInventory(AbilitySo _ability)
        {
            _availableAbilities.Remove(_ability);
        }


        [ContextMenu("Use Current Ability")]
        private void UseAbility()
        {
            if (_currentAbility != null)
            {
                if(_currentAbility.IsReadyToUse())
                    _currentAbility.UseAbility(playerTransform.transform);
            }
        }

        [ContextMenu("Cycle")]
        public void CycleThroughAbilities()
        {
            if(_currentAbility != null)
            {
                if (_currentAbility.InUse()) return;
            }

            _currentAbilityIndex++;

            if(_currentAbilityIndex >= _availableAbilities.Count)
            {
                _currentAbilityIndex = 0;
            }

            if (_currentAbilityIndex < 0)
            {
                _currentAbilityIndex = _availableAbilities.Count - 1;
            }

            _currentAbility = _availableAbilities[_currentAbilityIndex];

            _currentAbility.InitializeAbility();
        }

        private void UpdateDurations()
        {
            for (int i = 0; i < _availableAbilities.Count; i++)
            {
                if (_availableAbilities[i].InUse())
                    _availableAbilities[i].UpdateDuration();
            }
        }

        private void UpdateCoolDowns()
        {
            for (int i = 0; i < _availableAbilities.Count; i++)
            {
                if (_availableAbilities[i].IsInCoolDown())
                    _availableAbilities[i].UpdateCoolDownTime();
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

        [Range(5, 30)]
        public int newDamageValue;
    }

    [System.Serializable]
    public struct TestAbilityProperties
    {
        public string message;
    }
}