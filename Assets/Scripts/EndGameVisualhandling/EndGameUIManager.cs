using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreenObject;
    [SerializeField] private GameObject defeatScreenObject;


    private void Start()
    {
        EndGameEventsManager.OnVictoryAchieved += ShowVictory;
        EndGameEventsManager.OnDefeatAchieved += ShowDefeat;
    }

    private void OnDestroy()
    {
        EndGameEventsManager.OnVictoryAchieved -= ShowVictory;
        EndGameEventsManager.OnDefeatAchieved -= ShowDefeat;
    }


    [ContextMenu("ShowVictory")]
    private void ShowVictory()
    {
        victoryScreenObject.SetActive(true);
    }

    [ContextMenu("HideVictory")]
    private void HideVictory()
    {
        victoryScreenObject.SetActive(false);
    }

    [ContextMenu("ShowDefeat")]
    private void ShowDefeat()
    {
        defeatScreenObject.SetActive(true);
    }

    [ContextMenu("HideDefeat")]
    private void HideDefeat()
    {
        defeatScreenObject.SetActive(false);
    }
}
