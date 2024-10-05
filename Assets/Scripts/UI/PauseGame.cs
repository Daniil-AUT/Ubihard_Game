using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include this for TextMeshPro

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Reference to the pause menu panel
    public Button resumeButton; // Reference to the TextMeshPro resume button
    public Button quitButton; // Reference to the TextMeshPro quit button (optional)
    
    private bool isPaused = false; // To track if the game is paused

    void Start()
    {
        // Ensure the pause menu is hidden at the start
        pauseMenuPanel.SetActive(false);

        // Add listeners to the buttons
        resumeButton.onClick.AddListener(ResumeGame);
        if (quitButton != null) 
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true); // Show the pause menu
        Time.timeScale = 0; // Pause the game time
        isPaused = true; // Update the pause state
        
        // Enable mouse cursor and visibility
        Cursor.lockState = CursorLockMode.None; // Allow mouse movement
        Cursor.visible = true; // Show the cursor
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false); // Hide the pause menu
        Time.timeScale = 1; // Resume the game time
        isPaused = false; // Update the pause state
        
        // Disable mouse cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock mouse to center
        Cursor.visible = false; // Hide the cursor
    }

    public void QuitGame()
    {
        // Add your logic for quitting the game (e.g., load main menu scene or exit)
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
        #endif
    }
}
