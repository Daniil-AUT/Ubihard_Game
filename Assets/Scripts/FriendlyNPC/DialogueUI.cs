using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    // have one or more of the dialogue classes
    public static DialogueUI Instance { get; private set; }

    // have a name of the npc, their text, and the next button
    public TextMeshProUGUI nameText; 
    public TextMeshProUGUI contentText; 
    public Button nextButton; 
    private List<string> contentList;
    private int contentIndex = 0;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);
        Hide();  
    }
    // show the details of the npc gui and unlock the mouse to use
    public void Show(string npcName, string[] content)
    {
        nameText.text = npcName;
        contentList = new List<string>(content);
        contentIndex = 0;
        contentText.text = contentList[contentIndex];
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // hide the gui given a condition and lock the mouse
    public void Hide()
    {
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // make the player continue clicking 'next' until there is no content left
    private void OnNextButtonClick()
    {
        contentIndex++;
        if (contentIndex >= contentList.Count)
        {
            Hide();
        }
        else
        {
            contentText.text = contentList[contentIndex];
        }
    }
}
