using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI; // For button components

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button optionsButton; // Optional if you have a third button

    void Start()
    {
        // Add listeners for button clicks
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);

        // Optionally add listener for the third button
        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(OptionsClicked);
        }
    }

    void StartGame()
    {
        // Load the actual scene
        SceneManager.LoadScene("Actual Scene");
    }

    void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }

    void OptionsClicked()
    {
        // Handle options button click (if applicable)
        Debug.Log("Options button clicked");
    }
}
