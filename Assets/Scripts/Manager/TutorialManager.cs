using UnityEngine;
using TMPro;  // Import the TextMeshPro namespace for handling TextMeshProUGUI elements

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;        // Reference to the tutorial panel
    public TextMeshProUGUI tutorialText;    // Reference to the tutorial TextMeshProUGUI (showing 'H - Tutorial')
    public TextMeshProUGUI escapeText;      // Reference to the escape TextMeshProUGUI (showing 'Esc - Close')
    public Camera playerCamera;            // Reference to the player camera (optional)

    void Start()
    {
        // Ensure the tutorial panel and texts are initially inactive
        tutorialPanel.SetActive(false);
        tutorialText.gameObject.SetActive(true);  // Show the tutorial text box at the start
        escapeText.gameObject.SetActive(false);   // Hide the escape text box at the start
    }

    void Update()
    {
        // Check if the 'H' key is pressed to show the tutorial
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowTutorial();
        }

        // Check if the 'Esc' key is pressed to close the tutorial
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTutorial();
        }
    }

    // Method to show the tutorial and update text visibility
    void ShowTutorial()
    {
        if (playerCamera == null || playerCamera.gameObject.activeInHierarchy)
        {
            tutorialPanel.SetActive(true);
            tutorialText.gameObject.SetActive(false);  // Hide the tutorial text box
            escapeText.gameObject.SetActive(true);     // Show the escape text box
        }
    }

    // Method to close the tutorial and update text visibility
    void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        tutorialText.gameObject.SetActive(true);  // Show the tutorial text box
        escapeText.gameObject.SetActive(false);   // Hide the escape text box
    }
}
