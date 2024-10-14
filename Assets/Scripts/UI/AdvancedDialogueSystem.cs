using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public List<string> responses;
}

[System.Serializable]
public class DialogueSequence
{
    public string initialPrompt;
    public List<DialogueOption> options;
}

public class AdvancedDialogueSystem : MonoBehaviour
{
    [SerializeField] private Text dialogueText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private GameObject dialoguePanel;

    private List<DialogueSequence> dialogueSequences = new List<DialogueSequence>();
    private int currentSequenceIndex = 0;
    private int currentResponseIndex = 0;

    private void Start()
    {
        HideDialoguePanel();
    }

    public void StartDialogue(List<DialogueSequence> sequences)
    {
        dialogueSequences = sequences;
        currentSequenceIndex = 0;
        ShowDialoguePanel();
        DisplayCurrentSequence();
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
            if (i < currentSequence.options.Count)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Text>().text = currentSequence.options[i].optionText;
                int optionIndex = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SelectOption(optionIndex));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void SelectOption(int optionIndex)
    {
        DialogueOption selectedOption = dialogueSequences[currentSequenceIndex].options[optionIndex];
        currentResponseIndex = 0;
        DisplayNextResponse(selectedOption);
    }

    private void DisplayNextResponse(DialogueOption option)
    {
        if (currentResponseIndex >= option.responses.Count)
        {
            currentSequenceIndex++;
            DisplayCurrentSequence();
            return;
        }

        dialogueText.text = option.responses[currentResponseIndex];
        currentResponseIndex++;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i == 0)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Text>().text = "Continue";
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => DisplayNextResponse(option));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void EndDialogue()
    {
        HideDialoguePanel();
        // You can add any additional logic here for when the dialogue ends
    }

    private void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    private void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
    }

    // Example method to create a dialogue sequence
    public static DialogueSequence CreateDialogueSequence(string initialPrompt, params (string optionText, string[] responses)[] options)
    {
        return new DialogueSequence
        {
            initialPrompt = initialPrompt,
            options = options.Select(o => new DialogueOption
            {
                optionText = o.optionText,
                responses = new List<string>(o.responses)
            }).ToList()
        };
    }
}