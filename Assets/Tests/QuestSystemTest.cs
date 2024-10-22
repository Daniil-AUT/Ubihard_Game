using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic; // Needed for HashSet

public class QuestSystemTest
{
    public GameObject npcObjectGameObject;
    public NPCObject npcObject;
    public GameObject playerGameObject;
    public PlayerController playerController;

    public QuestPanelSystem questPanelSystem;
    public NotificationPanel notificationPanel;
    public DialogueUI dialogueUI;

    [SetUp]
    public void SetUp()
    {
        // Set up the NPC object
        npcObjectGameObject = new GameObject();
        npcObject = npcObjectGameObject.AddComponent<NPCObject>();

        // Create and set up the player
        playerGameObject = new GameObject();
        playerController = playerGameObject.AddComponent<PlayerController>();
        playerGameObject.tag = "Player"; // Set the player's tag

        // Create required components
        questPanelSystem = npcObjectGameObject.AddComponent<QuestPanelSystem>();
        notificationPanel = npcObjectGameObject.AddComponent<NotificationPanel>();
        dialogueUI = npcObjectGameObject.AddComponent<DialogueUI>();

        npcObject.questPanelSystem = questPanelSystem;
        npcObject.notificationPanel = notificationPanel;
        DialogueUI.Instance = dialogueUI;
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(npcObjectGameObject);
        GameObject.DestroyImmediate(playerGameObject);
    }

    // Acceptance Test (1a): Check if the player can see the NPC and approach it.
    [Test]
    public void PlayerEntersNPCZone_ShowsStartNotification()
    {
        npcObject.isQuestGiver = true;
        npcObject.quest = new QuestPanelSystem.QuestInfo { questTitle = "Find the lost teddy bear.", isCompleted = false };

        // act
        npcObject.OnTriggerEnter(playerGameObject.AddComponent<BoxCollider>()); 

        // assert
        Assert.IsTrue(dialogueUI.questDialogueVisible);
    }

    // Acceptance Test (1b): Player interaction for accepting a quest
    [Test]
    public void PlayerInteractNPCGetQuest_QuestActivate()
    {
        npcObject.isQuestGiver = true;
        npcObject.quest = new QuestPanelSystem.QuestInfo { questTitle = "Find the lost teddy bear.", isCompleted = false };

        // act
        npcObject.Interact();

        // assert
        Assert.AreEqual(questPanelSystem.currentQuest.questTitle, npcObject.quest.questTitle);
    }

    // Acceptance Test (2a): Player interaction when the quest is already completed
    [Test]
    public void PlayerInteractNPC_QuestCompleted()
    {
        npcObject.isQuestGiver = true;
        npcObject.quest = new QuestPanelSystem.QuestInfo { questTitle = "Find the lost teddy bear.", isCompleted = true };

        // Act
        npcObject.Interact();

        // Assert
        Assert.IsFalse(dialogueUI.questDialogueVisible);
    }

    // Acceptance Test (2b): Notification to give a teddy bear
    [Test]
    public void PlayerEnterNPCZoneWithItem_ShowNotification()
    {
        playerController.HasItem("Teddy Bear"); 

        // acta
        npcObject.OnTriggerEnter(playerGameObject.AddComponent<BoxCollider>()); 

        // assert
        Assert.IsFalse(notificationPanel.notificationVisible);
    }

    public class NPCObject : MonoBehaviour
    {
        public bool isQuestGiver;
        public QuestPanelSystem.QuestInfo quest;
        public QuestPanelSystem questPanelSystem;
        public NotificationPanel notificationPanel;

        public void OnTriggerEnter(Collider other)
        {
            // Basic logic for NPC interaction
            if (isQuestGiver && other.CompareTag("Player"))
            {
                // Show the quest dialogue
                DialogueUI.Instance.ShowQuestDialogue(quest.questTitle, "Please help me find my teddy bear!");

                // Check if the player has the required item
                if (other.GetComponent<PlayerController>().HasItemCheck("Teddy Bear"))
                {
                    notificationPanel.ShowNotification("Thank you for bringing the teddy bear!", 3f); // Show notification
                }
            }
        }

        public void Interact()
        {
            if (isQuestGiver && quest != null)
            {
                if (!quest.isCompleted)
                {
                    questPanelSystem.ActivateQuest(quest);
                }
                else
                {
                    DialogueUI.Instance.Hide();
                }
            }
        }
    }

    public class PlayerController : MonoBehaviour
    {
        public HashSet<string> items = new HashSet<string>();

        public void HasItem(string item)
        {
            items.Add(item);
        }

        public bool HasItemCheck(string item)
        {
            return items.Contains(item);
        }
    }

    public class QuestPanelSystem : MonoBehaviour
    {
        public QuestInfo currentQuest;

        public class QuestInfo
        {
            public string questTitle;
            public bool isCompleted;
        }

        public void ActivateQuest(QuestInfo quest)
        {
            currentQuest = quest;
        }
    }

    public class NotificationPanel : MonoBehaviour
    {
        public bool notificationVisible;

        public void ShowNotification(string message, float duration)
        {
            notificationVisible = true;
        }

        public void HideNotification()
        {
            notificationVisible = false;
        }
    }

    public class DialogueUI : MonoBehaviour
    {
        public static DialogueUI Instance;
        public bool questDialogueVisible;

        public void ShowQuestDialogue(string title, string description)
        {
            questDialogueVisible = true;
        }

        public void Hide()
        {
            questDialogueVisible = false;
        }
    }
}
