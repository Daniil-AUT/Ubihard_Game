using UnityEngine;

// Use the interactable object class to see if the player is inside the radius
public class NPCObject : NPCInteractableObject
{
    public string npcName;
    public string[] contentList;
    public bool isQuestGiver;
    public QuestPanelSystem.QuestInfo quest; // Use the QuestInfo class directly

    private bool playerInRange = false;

    // If the player collides with an NPC/their zone then show log it and show UI
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Player entered the radius");
            playerInRange = true;

            // Show dialogue with quest options if NPC is a quest giver
            if (isQuestGiver && quest != null && !quest.isCompleted)
            {
                // Show quest dialogue
                DialogueUI.Instance.ShowQuestDialogue(quest.questTitle, quest.questDescription);
            }
            else
            {
                DialogueUI.Instance.Show(npcName, contentList);
            }
        }
    }

    // Use the Player Controller script to see if the player is outside the radius to remove UI
    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Player exited");
            playerInRange = false;
            DialogueUI.Instance.Hide(); // Hide dialogue but NOT the NPC
        }
    }

    protected override void Interact()
    {
        if (playerInRange)
        {
            // Show dialogue with quest options if NPC is a quest giver
            if (isQuestGiver && quest != null && !quest.isCompleted)
            {
                DialogueUI.Instance.ShowQuestDialogue(quest.questTitle, quest.questDescription);
            }
            else
            {
                DialogueUI.Instance.Show(npcName, contentList);
            }
        }
    }
}
