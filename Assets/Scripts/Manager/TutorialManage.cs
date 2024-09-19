using UnityEngine;
using UnityEngine.UI;

public class TutorialManage : MonoBehaviour
{
    public GameObject tutorialPanel;  
    public GameObject indicatorImage;
    public Button closeButton;       

    private bool isTutorialOpen = false;  

    void Start()
    {
        tutorialPanel.SetActive(false);  
        indicatorImage.SetActive(true);  
        Cursor.lockState = CursorLockMode.Locked;  
        Cursor.visible = false;
        closeButton.onClick.AddListener(CloseTutorial);
    }

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

    public void OpenTutorial()
    {
        isTutorialOpen = true;
        tutorialPanel.SetActive(true); 
        indicatorImage.SetActive(false);  
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void CloseTutorial()
    {
        isTutorialOpen = false;
        tutorialPanel.SetActive(false); 
        indicatorImage.SetActive(true); 
        Cursor.lockState = CursorLockMode.Locked;  
        Cursor.visible = false;
    }
}
