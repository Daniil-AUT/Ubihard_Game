using UnityEngine;
using TMPro;  // Import TextMeshPro namespace
using UnityEngine.UI;  // For Button component

public class TutorialManage : MonoBehaviour
{
    public GameObject tutorialPanel;               // Reference to the tutorial UI panel
    public TextMeshProUGUI[] textInstances;        // Array of TextMeshProUGUI components to hide/show
    public Button closeButton;                     // Reference to the Close button in the tutorial

    private bool isTutorialOpen = false;

    void Start()
    {
        // Ensure the tutorial and text instances are in the correct initial state
        tutorialPanel.SetActive(false);
        SetTextInstancesActive(true);  // Show all text instances initially

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
        SetTextInstancesActive(false);  // Hide all text instances when tutorial is opened

        // Enable the cursor for tutorial screen interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseTutorial()
    {
        isTutorialOpen = false;
        tutorialPanel.SetActive(false);
        SetTextInstancesActive(true);  // Show all text instances when tutorial is closed

        // Lock and hide the cursor when the tutorial is closed
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetTextInstancesActive(bool isActive)
    {
        foreach (TextMeshProUGUI textInstance in textInstances)
        {
            if (textInstance != null)
            {
                textInstance.gameObject.SetActive(isActive);
            }
        }
    }
}
