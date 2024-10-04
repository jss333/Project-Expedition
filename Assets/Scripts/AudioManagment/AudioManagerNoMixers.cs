using System;
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

    
    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    //private List<AudioClip> sfxAudioClips;
    [SerializeField] private List<SFXAudioDataSO> sFXAudioDataSOs;
    private List<string> sFXAudioNames = new List<string>();

    [Space(10)]
    [Header("Music")]
    [SerializeField] private bool muteMusic;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<AudioClip> musicAudioClips;

    private readonly float maxVolume = 0.5f;
    private bool isMusicOn = true;
    private float currentMusicLength;
    private int currentMusicIndex = 0;
    private float startFadeOutAt_Percentage = 0.8f;
    private float decreaseVolumeBy;
    private float currentMusicTime;

    private void Start()
    {
        for (int i = 0; i < sFXAudioDataSOs.Count; i++)
        {
            sFXAudioNames.Add(sFXAudioDataSOs[i].SFXClipName);
        }
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.H))
        {
            PlaySFXBasedOnSO("charge");
        } 
        
        if(Input.GetKeyDown(KeyCode.J))
        {
            PlaySFXBasedOnSO("chime");
        } 
        
        if(Input.GetKeyDown(KeyCode.K))
        {
            PlaySFXBasedOnSO("click");
        }
    }

    public void PlaySFXBasedOnSO(string audioClip)
    {
        int audioClipIndex = sFXAudioNames.IndexOf(audioClip);

        sfxSource.pitch = sFXAudioDataSOs[audioClipIndex].GetRandomFloat();
        sfxSource.PlayOneShot(sFXAudioDataSOs[audioClipIndex].audioClip, 1);
    }

    public void PlaySFX(SFX sfx)
    {
        //sfxSource.PlayOneShot(sfxAudioClips[(int)sfx], 1);
    }

    public void RunNextMusicClip()
    {
        currentMusicIndex++;
        if (currentMusicIndex >= musicAudioClips.Count)
            currentMusicIndex = 0;

        musicSource.clip = musicAudioClips[currentMusicIndex];
        currentMusicLength = musicAudioClips[currentMusicIndex].length;
        decreaseVolumeBy = maxVolume / ((1 - startFadeOutAt_Percentage) * musicAudioClips[currentMusicIndex].length);
        musicSource.volume = 0;
        musicSource.Play();
    }

    [ContextMenu("CalmMusic")]
    public void PlayCalmMusic()
    {
        musicSource.clip = musicAudioClips[0];
        currentMusicLength = musicAudioClips[0].length;
        //decreaseVolumeBy = maxVolume / ((1 - startFadeOutAt_Percentage) * musicAudioClips[0].length);
        musicSource.volume = PlayerPrefs.GetFloat("SFXVol");
        musicSource.Play();
    }

    [ContextMenu("ActionMusic")]
    public void PlayActionMusic()
    {
        musicSource.clip = musicAudioClips[1];
        currentMusicLength = musicAudioClips[1].length;
        //decreaseVolumeBy = maxVolume / ((1 - startFadeOutAt_Percentage) * musicAudioClips[1].length);
        musicSource.volume = PlayerPrefs.GetFloat("MusicVol");
        musicSource.Play();
    }

    public void OnButtonClick_ButtonClickSound()
    {
        PlaySFX(SFX.SFX_ButtonClick);
    }

    public void OnButtonClick_Activate()
    {
        PlaySFX(SFX.SFX_OrbCollected);
    }

    void PlayEnemyDeathAudio()
    {
        PlaySFX(SFX.SFX_EnemyDeath);
    }


    public void LoadScriptableObjects()
    {
        sFXAudioDataSOs.Clear(); // Clear the list before loading new items
        SFXAudioDataSO[] objects = Resources.LoadAll<SFXAudioDataSO>("SFXSOs");
        sFXAudioDataSOs.AddRange(objects);
        Debug.Log("ScriptableObjects loaded: " + sFXAudioDataSOs.Count);
    }
}