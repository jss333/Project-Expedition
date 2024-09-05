using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitchAudioSource : MonoBehaviour
{

    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    private AudioSource audioSource;
    private System.Random random;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        random = new System.Random();
    }

    public void PlayAudioWithNormalPitch(AudioClip clip)
    {
        PlayAudio(clip, 1f);
    }

    public void PlayAudioWithRandomPitch(AudioClip clip)
    {
        PlayAudio(clip, GetRandomFloat(minPitch, maxPitch));
    }

    public void PlayAudio(AudioClip clip, float pitch)
    {
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip);
    }

    private float GetRandomFloat(float min, float max)
    {
        return (float)(min + (random.NextDouble() * (max - min)));
    }
}
