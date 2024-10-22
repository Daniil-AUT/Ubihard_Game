using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; 

[System.Serializable]
public class DialogueOption
{
    public string buttonText;
    public string responseText;
    public int nextSequenceIndex;
}

[System.Serializable]
public class DialogueSequence
{
    public string initialPrompt;
    public DialogueOption[] options;
}

public class AdvancedDialogueSystem : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dialogueText; 
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TextMeshProUGUI[] buttonTexts; 
    [SerializeField] private Button closeButton; 

    [Header("Dialogue Data")]
    [SerializeField] private List<DialogueSequence> dialogueSequences;

    [Header("Player Detection")]
    public string playerTag = "Player";

    [Header("Timing")]
    [SerializeField] private float responseDisplayTime = 2f;

    private int currentSequenceIndex = 0;

    private void Start()
    {
        HideDialoguePanel();
        SetupButtonListeners();
        closeButton.onClick.AddListener(EndDialogue); 
        Cursor.visible = false; // Ensure cursor is hidden at the start
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    private void SetupButtonListeners()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; 
            optionButtons[i].onClick.AddListener(() => SelectOption(index));
        }
    }

    public void StartDialogue()
    {
        currentSequenceIndex = 0;
        ShowDialoguePanel();
        DisplayCurrentSequence();
        EnableCursor(); // Enable cursor when starting dialogue
    }

    private void DisplayCurrentSequence()
    {
        if (currentSequenceIndex >= dialogueSequences.Count)
        {
            EndDialogue();
            return;
        }

        DialogueSequence currentSequence = dialogueSequences[currentSequenceIndex];
        dialogueText.text = currentSequence.initialPrompt;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentSequence.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                buttonTexts[i].text = currentSequence.options[i].buttonText;
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }

        closeButton.gameObject.SetActive(true);
    }

    private void SelectOption(int optionIndex)
    {
        DialogueSequence currentSequence = dialogueSequences[currentSequenceIndex];
        DialogueOption selectedOption = currentSequence.options[optionIndex];

        dialogueText.text = selectedOption.responseText; 
        SetButtonsActive(false);

        if (selectedOption.nextSequenceIndex >= 0)
        {
            currentSequenceIndex = selectedOption.nextSequenceIndex;
            Invoke(nameof(DisplayCurrentSequence), responseDisplayTime);
        }
        else
        {
            Invoke(nameof(EndDialogue), responseDisplayTime);
        }
    }

    private void EndDialogue()
    {
        HideDialoguePanel();
        DisableCursor(); // Disable cursor when ending dialogue
    }

    private void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    private void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
        SetButtonsActive(false);
    }

    private void SetButtonsActive(bool active)
    {
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(active);
        }
        closeButton.gameObject.SetActive(active);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            StartDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            EndDialogue();
        }
    }

    private void EnableCursor()
    {
        Cursor.visible = true; // Show the cursor
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
    }

    private void DisableCursor()
    {
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again
    }
}
