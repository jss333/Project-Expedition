using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreenObject;
    [SerializeField] private GameObject defeatScreenObject;

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
        victoryScreenObject.SetActive(true);
    }

    private void HideVictory()
    {
        victoryScreenObject.SetActive(false);
    }

    private void ShowDefeat()
    {
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