using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    //[CreateAssetMenu(fileName = "New BombThrow", menuName = "Ability System/BombThrow")]

    public class BombThrow : AbilitySo
    {
        public override void UseAbility(Transform spawnPoint)
        {
            duration = 0;

        }
        public override void ForceCancelAbility()
        {
            throw new NotImplementedException();
        }

        public override void UpdateCoolDownTime()
        {
            throw new NotImplementedException();
        }

        public override void UpdateDuration()
        {
            throw new NotImplementedException();
        }

        
    }
}

