using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] // This is for if you want to input any value
    private AudioClip select_clip;

    [SerializeField]
    private AudioClip back_clip;

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject InGameDebugObject;

    private bool isPaused = false;

    public delegate void TogglePause();
    public static TogglePause togglePause;
    private static PauseMenu instance;

    [SerializeField]
    private InputActionAsset playerActions;
    private InputActionMap playerActionMap;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        pauseMenuUI.SetActive(false);

        playerActionMap = playerActions.FindActionMap("Player");

    }
    private void Start()
    {
        InGameDebugObject.SetActive(false);
    }

    private void OnEnable()
    {
        togglePause += HandlePause;
    }
    private void OnDisable()
    {
        togglePause -= HandlePause;
    }
    public void HandlePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; //This will resume
        isPaused = false;
        //playerActionMap.Enable();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        playerActionMap.Disable();
        Time.timeScale = 0f; // This will pause the game
        isPaused = true;
    }

    public void LoadMenu()
    {

        Resume();
        SceneManager.LoadScene(0);

    }
    public void QuitGame()
    {
        // You can add more logic here like confirmation dialog if needed
        Application.Quit();
    }


    //public void PlaySelectClip()
    //{
    //    MusicManager.instance.PlayOneShotClip(select_clip);
    //}

    //public void PlayBackClip()
    //{
    //    MusicManager.instance.PlayOneShotClip(back_clip);
    //}

}


//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

//public class PauseMenu : MonoBehaviour
//{
//    [SerializeField] private AudioClip select_clip;
//    [SerializeField] private AudioClip back_clip;
//    [SerializeField] private GameObject pauseMenuUI;
//    [SerializeField] private GameObject InGameDebugObject;

//    private bool isPaused = false;

//    public delegate void TogglePause();
//    public static TogglePause togglePause;
//    private static PauseMenu instance;

//    [SerializeField] private InputActionAsset playerActions;
//    private InputActionMap playerActionMap;

//    private void Awake()
//    {
//        if (instance != null)
//        {
//            Destroy(gameObject);
//        }
//        else
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        pauseMenuUI.SetActive(false); // Ensure pause menu is hidden at start
//        playerActionMap = playerActions.FindActionMap("Player");
//    }

//    private void Start()
//    {
//        InGameDebugObject.SetActive(false); // Ensure debug object is hidden
//    }

//    private void Update()
//    {
//        // Listen for the Esc key press
//        if (Keyboard.current.escapeKey.wasPressedThisFrame)
//        {
//            HandlePause(); // Call the pause handler when Esc is pressed
//        }
//    }

//    private void OnEnable()
//    {
//        togglePause += HandlePause;
//    }

//    private void OnDisable()
//    {
//        togglePause -= HandlePause;
//    }

//    public void HandlePause()
//    {
//        if (isPaused)
//            Resume();
//        else
//            Pause();
//    }

//    public void Resume()
//    {
//        pauseMenuUI.SetActive(false); // Hide pause menu
//        Time.timeScale = 1f;          // Resume game time
//        isPaused = false;             // Set pause state to false
//    }

//    public void Pause()
//    {
//        pauseMenuUI.SetActive(true);  // Show pause menu
//        playerActionMap.Disable();    // Disable player input
//        Time.timeScale = 0f;          // Pause game time
//        isPaused = true;              // Set pause state to true
//    }

//    public void LoadMenu()
//    {
//        Resume(); // Unpause before loading the main menu
//        SceneManager.LoadScene(0); // Assuming scene 0 is the main menu
//    }

//    public void QuitGame()
//    {
//        Application.Quit(); // Quits the game (works in build, not in editor)
//    }
//}
