using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the UI GameObject
    private bool isPaused;

    void Update()
    {
        // Toggle pause when the "Esc" key is pressed

        if (isPaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
    }
    // Resume the game
    public void Resume()
    {
        //pauseMenuUI.SetActive(false);  // Hide the pause menu
        //Time.timeScale = 1f;           // Resume time
        isPaused = false;
    }

    // Pause the game
    //public void Pause()
    //{
    //    pauseMenuUI.SetActive(true);   // Show the pause menu
    //    Time.timeScale = 0f;           // Freeze time
    //    isPaused = true;
    //}

    // Optional: Method to call when a "Quit" button is clicked
    public void QuitGame()
    {
        Application.Quit();            // Quit the game
        Debug.Log("Game is quitting...");  // For debugging in the editor
    }
}
