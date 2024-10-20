using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

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
    [SerializeField] private GameObject soundEffectPrefab;
    private ObjectPool<SoundEffect> sfxPool;

    [Header("SFX")]
    [SerializeField] private int sfxVolume = 8;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource shootSfxSource;
    private Dictionary<string, SFXAudioDataSO> sfxAudioClipMap = new Dictionary<string, SFXAudioDataSO>();

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
        sfxPool = new ObjectPool<SoundEffect>(CreateSoundEffect, ActionOnGet, ActionOnRelease, ActionOnDestroy);

        LoadSFXScriptableObjects();
    }

    private SoundEffect CreateSoundEffect()
    {
        return null;
    }

    private void ActionOnGet(SoundEffect effect)
    {
        effect.gameObject.SetActive(true);
    }

    private void ActionOnRelease(SoundEffect effect)
    {
        effect.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(SoundEffect effect)
    {
        Destroy(effect.gameObject);
    }


    public void LoadSFXScriptableObjects()
    {
        SFXAudioDataSO[] sfxAudioDataSOs = Resources.LoadAll<SFXAudioDataSO>("SFXSOs");
        foreach (SFXAudioDataSO audioDataSO in sfxAudioDataSOs)
        {
            sfxAudioClipMap.Add(audioDataSO.SFXClipName, audioDataSO);
        }
        Debug.Log("SFXAudioDataSOs loaded: " + sfxAudioClipMap.Count);
    }

    public void PlaySFXByName(string sfxAudioClipName)
    {
        SFXAudioDataSO clipToPlay = GetSFXSOFromName(sfxAudioClipName);
        PlaySFX(clipToPlay);
        /*sfxSource.pitch = clipToPlay.GetRandomPitch();
        sfxSource.PlayOneShot(clipToPlay.audioClip, 1);
        sfxSource.pitch = 1; */
    }

    private void PlaySFX(SFXAudioDataSO sFXAudioDataSO)
    {
        AudioSource sfxAudioSource = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == sFXAudioDataSO.name)
            {
                sfxAudioSource = transform.GetChild(i).GetComponent<AudioSource>();
                break;
            }
            
            if(i == transform.childCount - 1)
            {
                GameObject newAudioSourceInstance = Instantiate(soundEffectPrefab, transform);
                newAudioSourceInstance.name = sFXAudioDataSO.SFXClipName;
                sfxAudioSource = newAudioSourceInstance.GetComponent<AudioSource>();

                break;
            }
        }
        
        if(sfxAudioSource != null )
        {
            sfxAudioSource.pitch = sFXAudioDataSO.GetRandomPitch();
            sfxAudioSource.volume = sFXAudioDataSO.soundEffectVolume;
            sfxAudioSource.PlayOneShot(sFXAudioDataSO.audioClip);
        }

        //shootSfxSource.pitch = sFXAudioDataSO.GetRandomPitch();
        //shootSfxSource.volume = sFXAudioDataSO.soundEffectVolume;
        //shootSfxSource.clip = sFXAudioDataSO.audioClip;
        //shootSfxSource.PlayOneShot(sFXAudioDataSO.audioClip);

        /*SoundEffect soundEffect = sfxPool.Get();
        soundEffect.SetSound(sFXAudioDataSO);
        soundEffect.gameObject.SetActive(true);
        StartCoroutine(DisableSFX(soundEffect, sFXAudioDataSO.audioClip.length));  */
    }

    private IEnumerator DisableSFX(SoundEffect soundEffect, float length)
    {
        yield return new WaitForSeconds(length);
        sfxPool.Release(soundEffect);
    }

    public float LinearToDecibels(int linear)
    {
        float linearScaleRange = 20f;

        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
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

    private SFXAudioDataSO GetSFXSOFromName(string sfxName)
    {
        return sfxAudioClipMap[sfxName];
    }
}