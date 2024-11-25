using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreenObject;
    [SerializeField] private GameObject defeatScreenObject;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button retryLevelButton;

    private void Start()
    {
        EndGameEventManager.OnVictoryAchieved += ShowVictory;
        EndGameEventManager.OnDefeatAchieved += ShowDefeat;
    }

    private void OnDestroy()
    {
        EndGameEventManager.OnVictoryAchieved -= ShowVictory;
        EndGameEventManager.OnDefeatAchieved -= ShowDefeat;
    }


    private void ShowVictory()
    {
        nextLevelButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.AddListener(() => LevelManager.Singleton.LoadLevel(RoundManager.Singleton.RoundSettings.NextLevel));

        victoryScreenObject.SetActive(true);
    }

    private void HideVictory()
    {
        victoryScreenObject.SetActive(false);
    }

    private void ShowDefeat()
    {
        retryLevelButton.onClick.RemoveAllListeners();
        retryLevelButton.onClick.AddListener(() => LevelManager.Singleton.LoadLevel(RoundManager.Singleton.RoundSettings.CurrentLevel));

        defeatScreenObject.SetActive(true);
    }

    private void HideDefeat()
    {
        defeatScreenObject.SetActive(false);
    }

    
    public void Retry_UICallBack()
    {
        LevelManager.Singleton.LoadLevel(1);
    }

    public void Quit_UICallBack()
    {
        LevelManager.Singleton.LoadLevel(0);
    }
}