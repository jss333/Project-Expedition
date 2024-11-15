using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] private RectTransform settingsPanel;
    [SerializeField] private RectTransform settingsPanelBG;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sFXSlider;

    private Vector3 startScale;

    IEnumerator Start()
    {
        InputHandler.Singleton.OnUIMenuDeActivated += HideSettingsPanel;

        yield return null;

        startScale = settingsPanel.lossyScale;
        settingsPanel.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        InputHandler.Singleton.OnUIMenuDeActivated -= HideSettingsPanel;
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(true);

        /*LeanTween.cancel(settingsPanelBG);
        LeanTween.alpha(settingsPanelBG, 0.85f, 0.5f).setRecursive(false);   */

        /*Debug.Log(startScale);
        settingsPanel.localScale = startScale * 0.2f;

        LeanTween.cancel(settingsPanel);
        LeanTween.scale(settingsPanel, startScale, 0.5f).setEase(LeanTweenType.easeInCubic); */
    }

    public void HideSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(false);
    }

    public void ControlMusicVolume()
    {
        AudioManagerNoMixers.Singleton.ControlMusicVolume(musicSlider.value);
    }

    public void ControlSFXVolume()
    {
        AudioManagerNoMixers.Singleton.ControlSFXVolume(sFXSlider.value);
    }
}
