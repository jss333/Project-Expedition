using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeRoomBGM : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip firstHalfBGM;
    public AudioClip secondHalfBGM;
    public AudioClip victoryBGM;
    public AudioClip defeatBGM;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //StopAndPlayNewClip(firstHalfBGM);
    }

    public void PlaySecondHalfBGM()
    {
        StopAndPlayNewClip(secondHalfBGM);
    }
    public void PlayVictoryBGM()
    {
        StopAndPlayNewClip(victoryBGM);
    }

    public void PlayDefeatBGM()
    {
        StopAndPlayNewClip(defeatBGM);
    }

    private void StopAndPlayNewClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
