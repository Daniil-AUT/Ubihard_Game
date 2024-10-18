using UnityEngine;

public class NPCObject : NPCInteractableObject
{
    public string npcName;
    public string[] contentList;
    public bool isQuestGiver;
    public QuestPanelSystem.QuestInfo quest; 
    public NotificationPanel notificationPanel; 
    public QuestPanelSystem questPanelSystem; 

    private bool playerInRange = false;

    private void Start()
    {
        if (notificationPanel == null)
        {
            notificationPanel = FindObjectOfType<NotificationPanel>(); 
        }

        if (questPanelSystem == null)
        {
            questPanelSystem = FindObjectOfType<QuestPanelSystem>(); 
        }
    }

    // If the player collides with an NPC/their zone then show log it and show UI
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Player entered the radius");
            playerInRange = true;

            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI != null && InventoryManager.Instance.HasItem(inventoryUI.teddyBearItem) && DialogueUI.isDialogueFinished)
            {
                notificationPanel.ShowNotification("Press [E] to gift Teddy!", 2f); 
            }
            else if (!DialogueUI.isDialogueFinished)
            {
                notificationPanel.HideNotification();
            }
            else
            {
                notificationPanel.ShowNotification("You need to find a Teddy Bear to gift.", 2f); 
            }

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

    // Use the Player Controller script to see if the player is outside the radius to remove UI
    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Player exited");
            playerInRange = false;
            DialogueUI.Instance.Hide(); 
        }
    }

    public void CompleteDialogue()
    {
        if (questPanelSystem.currentQuest != null && !questPanelSystem.currentQuest.isCompleted)
        {
            questPanelSystem.ActivateQuest(questPanelSystem.currentQuest);
        }

        DialogueUI.isDialogueFinished = true; 
        UpdateInteractionText();
    }

    private void UpdateInteractionText()
    {
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();

        if (inventoryUI != null && InventoryManager.Instance.HasItem(inventoryUI.teddyBearItem) && DialogueUI.isDialogueFinished)
        {
            notificationPanel.ShowNotification("Press [E] to gift Teddy!", 3f); 
        }
        else if (!DialogueUI.isDialogueFinished)
        {
            notificationPanel.HideNotification();
        }
        else
        {
            notificationPanel.ShowNotification("You need to find a Teddy Bear to gift.", 3f); 
        }
    }

    protected override void Interact()
    {
        if (playerInRange && DialogueUI.isDialogueFinished)
        {
           
            InventoryUI.Instance.UseTeddyBear(); 
        }
        else if (playerInRange)
        {
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

    // New method to check if player is in range
    public bool IsPlayerInRange()
    {
        return playerInRange;
    }
}
