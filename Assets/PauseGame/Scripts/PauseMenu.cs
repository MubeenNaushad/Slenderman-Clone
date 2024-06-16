using UnityEngine;
using UnityEngine.UI; // Required for accessing UI elements
using UnityEngine.SceneManagement; // Required for scene management

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI GameObject
    public Button exitButton; // Reference to the exit button in the pause menu

    public Button restartButton; // Reference to the restart button in the pause menu

    public Button backToMainMenuButton;

    private bool isPaused = false; // Flag to track if the game is paused

    void Start()
    {
        // Add a listener to the exit and restart button
        exitButton.onClick.AddListener(QuitGame);
        restartButton.onClick.AddListener(RestartGame);
        backToMainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    void Update()
    {
        // Check if Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause state
            isPaused = !isPaused;

            // Activate/deactivate pause menu
            if (isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        // Show the pause menu UI
        pauseMenuUI.SetActive(true);

        // Pause the game by setting timescale to 0
        Time.timeScale = 0f;

        // Show and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Optional: Disable any other scripts that should not run while paused
        // Example: Disable player movement scripts, etc.
    }

    void ResumeGame()
    {
        // Hide the pause menu UI
        pauseMenuUI.SetActive(false);

        // Resume the game by setting timescale back to 1
        Time.timeScale = 1f;

        // Hide and lock the cursor (optional: you can choose to lock the cursor back to locked state)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Re-enable any scripts disabled when paused
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // If in the Unity Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If in a standalone build, quit the application
        Application.Quit();
#endif
    }

    public void RestartGame()
    {
        // Reload the current scene (restart the game)
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void LoadMainMenu()
    {
        // Load the main menu scene (assuming it's at build index 0)
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}