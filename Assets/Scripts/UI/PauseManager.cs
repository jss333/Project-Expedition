using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Singletone;

    private void Awake()
    {
        if (Singletone != null)
        {
            Destroy(Singletone);
        }
        else
        {
            Singletone = this;
        }
    }
    public void pauseGame(bool value)
    {
        if (value)
        {
            Time.timeScale = 0;
        }
        else 
        {
            Time.timeScale = 1; 
        }
    }
}
