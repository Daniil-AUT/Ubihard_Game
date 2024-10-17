using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class DialogueEntry
{
    public string npcText;
    public string[] buttonTexts;
    public int[] nextDialogueIndices;
}

public class SimplifiedDialogueSystem : MonoBehaviour
{
    [SerializeField] private Text npcTextBox;
    [SerializeField] private Button[] responseButtons;
    [SerializeField] private Text[] buttonTexts;

    [SerializeField] private List<DialogueEntry> dialogueEntries;

    private int currentDialogueIndex = 0;

    private void Start()
    {
        for (int i = 0; i < responseButtons.Length; i++)
        {
            int index = i;
            responseButtons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        StartDialogue();
    }

    private void StartDialogue()
    {
        currentDialogueIndex = 0;
        DisplayCurrentDialogue();
    }

    private void DisplayCurrentDialogue()
    {
        if (currentDialogueIndex >= dialogueEntries.Count)
        {
            EndDialogue();
            return;
        }

        DialogueEntry currentEntry = dialogueEntries[currentDialogueIndex];
        npcTextBox.text = currentEntry.npcText;

        for (int i = 0; i < responseButtons.Length; i++)
        {
            buttonTexts[i].text = currentEntry.buttonTexts[i];
            responseButtons[i].gameObject.SetActive(true);
        }
    }

    private void OnButtonClick(int buttonIndex)
    {
        int nextIndex = dialogueEntries[currentDialogueIndex].nextDialogueIndices[buttonIndex];

        if (nextIndex == -1)
        {
            EndDialogue();
        }
        else
        {
            currentDialogueIndex = nextIndex;
            DisplayCurrentDialogue();
        }
    }

    private void EndDialogue()
    {
        npcTextBox.text = "";
        foreach (Button button in responseButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}