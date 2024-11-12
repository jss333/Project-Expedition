using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the UI GameObject
    private bool isPaused;

    private void Start()
    {
        EndGameEventManager.OnVictoryAchieved += StopTime;
        EndGameEventManager.OnDefeatAchieved += StopTime;
        InputHandler.Singleton.OnHandlePausingAndResuming += HandlePausingAndResuming;
        isPaused = false;
        PauseManager.Singletone.pauseGame(false);
    }

    private void OnDestroy()
    {
        EndGameEventManager.OnVictoryAchieved -= StopTime;
        EndGameEventManager.OnDefeatAchieved -= StopTime;
        InputHandler.Singleton.OnHandlePausingAndResuming -= HandlePausingAndResuming;
    }
    void Update()
    {
        // Toggle pause when the "Esc" key is pressed

        if (isPaused)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            pauseMenuUI.SetActive(false);
        }
    }

    private void HandlePausingAndResuming()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            InputHandler.Singleton.OnUIMenuActivated?.Invoke();
            PauseManager.Singletone.pauseGame(true);
        }
        else
        {
            InputHandler.Singleton.OnUIMenuDeActivated?.Invoke();
            PauseManager.Singletone.pauseGame(false);
        }
    }

    // Resume the game
    public void Resume()
    {
        isPaused = false;
        InputHandler.Singleton.OnUIMenuDeActivated?.Invoke();
        PauseManager.Singletone.pauseGame(false);
    }

    // Optional: Method to call when a "Quit" button is clicked
    public void QuitGame()
    {
        Application.Quit();            // Quit the game
        Debug.Log("Game is quitting...");  // For debugging in the editor
    }

    private void StopTime()
    {
        PauseManager.Singletone.pauseGame(true);
    }

    public void LevelChange(string level)
    {
        SceneManager.LoadScene(level);
    }
}
