using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button resumeButton;
    public Button saveButton; // New Save button
    public Button loadButton; // New Load button
    public Button quitButton;
    
    private bool isPaused = false;

    void Start()
    {
        pauseMenuPanel.SetActive(false);

        resumeButton.onClick.AddListener(ResumeGame);
        saveButton.onClick.AddListener(SaveGame); // Add listener for Save button
        loadButton.onClick.AddListener(LoadGame); // Add listener for Load button
        if (quitButton != null) 
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    void Update()
    {
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
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SaveGame()
    {
        SaveLoadManager.Instance.SaveGame();
        Debug.Log("Game saved!");
        // Optionally, you can show a temporary message to the user that the game has been saved
    }

    public void LoadGame()
    {
        SaveLoadManager.Instance.LoadGame();
        Debug.Log("Game loaded!");
        // After loading, you might want to resume the game
        ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}