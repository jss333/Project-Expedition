using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class DefeatScreenManager : MonoBehaviour
{
    public GameObject defeatScreen; // Reference to the defeat screen UI

    // Method to reload the current scene
    public void Retry()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene(); // Get the current scene
        SceneManager.LoadScene(currentScene.buildIndex); // Reload the current scene
    }

    // Method to go back to the main menu
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0); // Load the main menu scene (adjust index as necessary)
    }
}
