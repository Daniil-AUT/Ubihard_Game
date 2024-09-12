using UnityEngine;
using UnityEngine.UI;  // Required for UI elements like buttons

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;   // Reference to the tutorial UI panel
    public GameObject imageWithText;   // Reference to the image with text (that you want to hide/show)
    public Button closeButton;         // Reference to the Close button in the tutorial

    private bool isTutorialOpen = false;

    void Start()
    {
        // Ensure the tutorial and image are in the correct initial state
        tutorialPanel.SetActive(false);
        imageWithText.SetActive(true);  // Show the image initially

        // Lock and hide the cursor initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Add a listener to the Close button so that it calls CloseTutorial when clicked
        closeButton.onClick.AddListener(CloseTutorial);
    }

    void Update()
    {
        // Toggle tutorial menu with the 'T' key (can be removed if not needed)
        if (Input.GetKeyDown(KeyCode.T) && !isTutorialOpen)
        {
            OpenTutorial();
        }
    }

    public void OpenTutorial()
    {
        isTutorialOpen = true;
        tutorialPanel.SetActive(true);
        imageWithText.SetActive(false);  // Hide the image when tutorial is opened

        // Enable the cursor for tutorial screen interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseTutorial()
    {
        isTutorialOpen = false;
        tutorialPanel.SetActive(false);
        imageWithText.SetActive(true);  // Show the image when tutorial is closed

        // Lock and hide the cursor when the tutorial is closed
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
