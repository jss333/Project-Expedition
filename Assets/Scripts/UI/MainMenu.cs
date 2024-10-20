using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        Destroy(FindObjectOfType<MainMenuBGM>().gameObject);
        SceneManager.LoadScene("Boss01");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
