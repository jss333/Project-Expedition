using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        audioSource.Play();
    }

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
        audioSource.Stop();
    }

    public void SetSound(SFXAudioDataSO sFXAudioDataSO)
    {
        audioSource.pitch = Random.Range(sFXAudioDataSO.minPitch, sFXAudioDataSO.maxPitch);
        audioSource.volume = sFXAudioDataSO.soundEffectVolume;
        //audioSource.clip = sFXAudioDataSO.audioClip;
    }
}
