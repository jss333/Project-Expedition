using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the UI GameObject
    private bool isPaused;

    private void Start()
    {
        isPaused = false;
        Time.timeScale = 1;

        EndGameEventsManager.OnVictoryAchieved += StopTime;
        EndGameEventsManager.OnDefeatAchieved += StopTime;
    }

    private void OnDestroy()
    {
        EndGameEventsManager.OnVictoryAchieved -= StopTime;
        EndGameEventsManager.OnDefeatAchieved -= StopTime;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    // Resume the game
    public void Resume()
    {
        isPaused = false;
    }

    // Optional: Method to call when a "Quit" button is clicked
    public void QuitGame()
    {
        Application.Quit();            // Quit the game
        Debug.Log("Game is quitting...");  // For debugging in the editor
    }

    private void StopTime()
    {
        Time.timeScale = 0;
    }
}
