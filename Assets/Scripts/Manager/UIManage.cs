using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button optionsButton; 

    void Start()
    {
    // Handle the different buttons in the main scene ui
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
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
        // Open the options button
        Debug.Log("Options button clicked");
    }
}
