using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }
    public static bool isDialogueFinished = false; 

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI contentText;
    public Button nextButton;
    private List<string> contentList;
    private int contentIndex = 0;

    public QuestPanelSystem questPanelSystem;

    // New variable to control if the dialogue is already shown
    private bool dialogueShown = false;

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

    public void Show(string npcName, string[] content)
    {
        // Only show dialogue if it hasn't been shown yet
        if (dialogueShown) return;

        nameText.text = npcName;
        contentList = new List<string>(content);
        contentIndex = 0;
        contentText.text = contentList[contentIndex];
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Mark dialogue as shown
        dialogueShown = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnNextButtonClick()
    {
        contentIndex++;
        if (contentIndex >= contentList.Count)
        {
            Hide();
            CompleteDialogue();
        }
        else
        {
            contentText.text = contentList[contentIndex];
        }
    }

    public void CompleteQuest()
    {
        Debug.Log("Quest has been completed.");
    }

    // New method to show quest dialogue
    public void ShowQuestDialogue(string questTitle, string questDescription)
    {
        nameText.text = questTitle;
        contentText.text = questDescription;
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // New method to complete the dialogue
    private void CompleteDialogue()
    {
        if (questPanelSystem.currentQuest != null && !questPanelSystem.currentQuest.isCompleted)
        {
            questPanelSystem.ActivateQuest(questPanelSystem.currentQuest);
        }

        isDialogueFinished = true;
    }
}
