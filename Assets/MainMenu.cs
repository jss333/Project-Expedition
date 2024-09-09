using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Button newGameButton;
    public Button loadGameButton;
    public Button quitButton;
    public Image pointerImage;  // Assign the pointer image in the inspector
    private Button currentButton;

    void Start()
    {
        // Add listeners for buttons
        newGameButton.onClick.AddListener(StartNewGame);
        loadGameButton.onClick.AddListener(LoadGame);
        quitButton.onClick.AddListener(QuitGame);

        // Hide the pointer image at the start
        pointerImage.enabled = false;
    }

    void Update()
    {
        // Check for hover events
        if (IsButtonHighlighted(newGameButton))
        {
            ShowPointer(newGameButton);
        }
        else if (IsButtonHighlighted(loadGameButton))
        {
            ShowPointer(loadGameButton);
        }
        else if (IsButtonHighlighted(quitButton))
        {
            ShowPointer(quitButton);
        }
        else
        {
            HidePointer();
        }
    }

    bool IsButtonHighlighted(Button button)
    {
        return EventSystem.current.currentSelectedGameObject == button.gameObject;
    }

    void ShowPointer(Button button)
    {
        if (currentButton != button)
        {
            currentButton = button;
            pointerImage.enabled = true;
            pointerImage.rectTransform.position = new Vector3(button.transform.position.x - 100f, button.transform.position.y, 0);  // Adjust pointer offset
        }
    }

    void HidePointer()
    {
        pointerImage.enabled = false;
        currentButton = null;
    }

    // Button actions
    void StartNewGame()
    {
        SceneManager.LoadScene("ChallengeRoom01");
    }

    void LoadGame()
    {
        Debug.Log("Load Game");
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
