using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerNoMixers : MonoBehaviour {

    #region Singleton

    public static AudioManagerNoMixers Singleton;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
        {
            if (Singleton != this)
                Destroy(gameObject);
        }
    }

    #endregion

    [Header("Pool")]
    [SerializeField] private GameObject randomPitchAudioSourcePrefab;

    [Header("SFX")]
    private Dictionary<string, SFXAudioDataSO> sfxSOByName = new Dictionary<string, SFXAudioDataSO>();
    private Dictionary<string, RandomPitchAudioSource> audioSrcByName = new Dictionary<string, RandomPitchAudioSource>();
    [SerializeField] private float sfxVolume = 1.0f;

    public float SFXVolume => sfxVolume;

    [Header("Music")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] AudioClip firstHalfBGM;
    [SerializeField] AudioClip secondHalfBGM;
    [SerializeField] AudioClip victoryBGM;
    [SerializeField] AudioClip defeatBGM;


    private void Start()
    {
        LoadSFXScriptableObjects();

        PlayFirstPartMusic();
    }

    public void LoadSFXScriptableObjects()
    {
        SFXAudioDataSO[] sfxAudioDataSOs = Resources.LoadAll<SFXAudioDataSO>("SFXSOs");
        foreach (SFXAudioDataSO audioDataSO in sfxAudioDataSOs)
        {
            sfxSOByName.Add(audioDataSO.name, audioDataSO);
        }
        Debug.Log("SFXAudioDataSOs loaded: " + sfxSOByName.Count);
    }

    public void PlaySFXByName(string sfxName)
    {
        if(!sfxSOByName.ContainsKey(sfxName))
        {
            Debug.LogError("Cannot play audio clip - SFXAudioDataSO with name " + sfxName + "not found");
            return;
        }

        RandomPitchAudioSource audioSrc = GetOrCreateAudioSource(sfxSOByName[sfxName]);
        audioSrc.PlayAssociatedAudio();
    }

    private RandomPitchAudioSource GetOrCreateAudioSource(SFXAudioDataSO sfxSO)
    {
        if (audioSrcByName.ContainsKey(sfxSO.name))
        {
            return audioSrcByName[sfxSO.name];
        }
        else
        {
            GameObject newAudioSrc = Instantiate(randomPitchAudioSourcePrefab, transform);
            newAudioSrc.name = sfxSO.name;

            RandomPitchAudioSource rndPitchAudioSrc = newAudioSrc.GetComponent<RandomPitchAudioSource>();
            rndPitchAudioSrc.SetAssociatedAudio(sfxSO);

            audioSrcByName.Add(sfxSO.name, rndPitchAudioSrc);
            return rndPitchAudioSrc;
        }
    }

    public void PlayFirstPartMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = firstHalfBGM;
        musicAudioSource.Play();
    }

    public void PlaySecondPartMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = secondHalfBGM;
        musicAudioSource.Play();
    }

    public void PlayVictroyMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = victoryBGM;
        musicAudioSource.Play();
    }

    public void PlayDefeatMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = defeatBGM;
        musicAudioSource.Play();
    }

    public void ControlMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }

    public void ControlSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}