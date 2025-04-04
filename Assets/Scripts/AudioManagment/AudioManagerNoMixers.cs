using Cinemachine.Examples;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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


    [Header("Music")]
    [SerializeField] private EventReference mainMenuMusic;
    [SerializeField] private EventReference firstHalfBGM;
    [SerializeField] private EventReference secondHalfBGM;
    [SerializeField] private EventReference victoryBGM;
    [SerializeField] private EventReference defeatBGM;

    [Header("Settings")]
    [SerializeField] private float musicVolume = 1.0f;
    [SerializeField] private float sfxVolume = 1.0f;
    private Bus musicBus;
    private Bus sfxBus;
    private EventInstance eventInstance;
    
    
    public float SFXVolume => sfxVolume;

    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();

        LoadSFXScriptableObjects();

        sfxVolume = 1.0f;

        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            PlayFirstPartMusic();
        }

        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void OnDestroy()
    {
        StopMusic();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        //worldPos = playerMovement.transform.position;
        RuntimeManager.PlayOneShot(sound, worldPos);
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
        /*if(!sfxSOByName.ContainsKey(sfxName))
        {
            Debug.LogError("Cannot play audio clip - SFXAudioDataSO with name " + sfxName + "not found");
            return;
        }

        RandomPitchAudioSource audioSrc = GetOrCreateAudioSource(sfxSOByName[sfxName]);
        audioSrc.PlayAssociatedAudio(); */
    }

    private RandomPitchAudioSource GetOrCreateAudioSource(SFXAudioDataSO sfxSO)
    {
        if (audioSrcByName.ContainsKey(sfxSO.name))
        {
            RandomPitchAudioSource rndPitchAudioSrc = audioSrcByName[sfxSO.name];
            rndPitchAudioSrc.SetAssociatedAudio(sfxSO);
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

    [ContextMenu("StopMusic")]
    private void StopMusic()
    {
        eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void PlayEventInstance(EventReference eventReference)
    {
        StopMusic();
        eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.start();
    }

    public void PlayMainMenuMusic()
    {
        PlayEventInstance(mainMenuMusic);
    }

    public void PlayFirstPartMusic()
    {
        PlayEventInstance(firstHalfBGM);
    }

    public void PlaySecondPartMusic()
    {
        PlayEventInstance(secondHalfBGM);
    }

    public void PlayVictroyMusic()
    {
        PlayEventInstance(victoryBGM);
    }

    public void PlayDefeatMusic()
    {
        PlayEventInstance(defeatBGM);
    }

    public void ControlMusicVolume(float volume)
    {
        musicVolume = volume;
        musicBus.setVolume(musicVolume);
    }

    public void ControlSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxBus.setVolume(sfxVolume);
    }
}