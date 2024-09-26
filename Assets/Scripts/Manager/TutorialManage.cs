using UnityEngine;
using UnityEngine.UI;

public class TutorialManage : MonoBehaviour
{
// link existing tutorial panel with contents in it.
// Also have an image for the indicator on how to open tutorial
// and have close button.
    public GameObject tutorialPanel;  
    public GameObject indicatorImage;
    public Button closeButton;       

    private bool isTutorialOpen = false;  

    // When game starts, make the tutorial panel invisible and the indicator
    // for how to open it visible
    void Start()
    {
        tutorialPanel.SetActive(false);  
        indicatorImage.SetActive(true);  
        Cursor.lockState = CursorLockMode.Locked;  
        Cursor.visible = false;
        closeButton.onClick.AddListener(CloseTutorial);
    }

    // if player presses the T open the tutorial
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isTutorialOpen)
            {
                CloseTutorial();  
            }
            else
            {
                OpenTutorial();  
            }
        }
    }

    // if tutorial is opened, the contents become visible and 
    // indicator becomes invisible
    public void OpenTutorial()
    {
        isTutorialOpen = true;
        tutorialPanel.SetActive(true); 
        indicatorImage.SetActive(false);  
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    // the close button will make contents invisible and indicator visible
    public void CloseTutorial()
    {
        isTutorialOpen = false;
        tutorialPanel.SetActive(false); 
        indicatorImage.SetActive(true); 
        Cursor.lockState = CursorLockMode.Locked;  
        Cursor.visible = false;
    }
}
