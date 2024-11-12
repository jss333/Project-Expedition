using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPitchAudioSource : MonoBehaviour
{
    private static System.Random random = new System.Random();

    public AudioClip clip;
    public float volume = 1;
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    [SerializeField] private AudioSource audioSource;

    public void SetAssociatedAudio(SFXAudioDataSO sfxDataSO)
    {
        minPitch = sfxDataSO.minPitch;
        maxPitch = sfxDataSO.maxPitch;
        volume = sfxDataSO.soundEffectVolume;
        clip = sfxDataSO.audioClip;

        audioSource.volume = volume;
    }

    public void PlayAssociatedAudio()
    {
        audioSource.pitch = GetRandomFloat(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }

    private float GetRandomFloat(float min, float max)
    {
        return (float)(min + (random.NextDouble() * (max - min)));
    }
}
