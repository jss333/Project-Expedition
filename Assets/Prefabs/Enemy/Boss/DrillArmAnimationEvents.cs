using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DrillArmAnimationEvents : MonoBehaviour
{
    [SerializeField] private EventReference bossArmGroundPunctureSFX;

    public void PlayBossArmGroundPunctureSFX()
    {
        AudioManagerNoMixers.Singleton.PlayOneShot(bossArmGroundPunctureSFX, this.transform.position);
    }
}
