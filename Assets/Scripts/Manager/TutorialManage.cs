using UnityEngine;
using TMPro;  
using UnityEngine.UI;

public class TutorialManage : MonoBehaviour
{
    // Reference the tutorial panel where content is hel
    public GameObject tutorialPanel;               
    public TextMeshProUGUI[] textInstances;        
    public Button closeButton;              

    // Set tutorial panel visibility to inviisble
    private bool isTutorialOpen = false;

    void Start()
    {
        tutorialPanel.SetActive(false);
        SetTextInstancesActive(true); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        closeButton.onClick.AddListener(CloseTutorial);
    }

    void Update()
    {
        // If the player presses T, the tutorial panel ui will open
        if (Input.GetKeyDown(KeyCode.T) && !isTutorialOpen)
        {
            OpenTutorial();
        }
    }

    // Handle logic for opening a tutorial like unlocking a mouse and making contents visible
    public void OpenTutorial()
    {
        isTutorialOpen = true;
        tutorialPanel.SetActive(true);
        SetTextInstancesActive(false);  
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Vice-versa logic as for the OpenTutorial
    public void CloseTutorial()
    {
        isTutorialOpen = false;
        tutorialPanel.SetActive(false);
        SetTextInstancesActive(true);  
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Manage visibility of the UI
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
