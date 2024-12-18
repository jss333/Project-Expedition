using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SFX", menuName = "Audio System/New SFX")]
public class SFXAudioDataSO : ScriptableObject
{
    public AudioClip audioClip;
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;
    public float soundEffectVolume = 1f;
}
