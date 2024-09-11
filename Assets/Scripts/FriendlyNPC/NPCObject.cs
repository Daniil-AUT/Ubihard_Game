using UnityEngine;

public class NPCObject : NPCInteractableObject
{
    [Header("NPC Details")]
    public string npcName;  // Assign via Inspector
    public string[] contentList;  // Assign via Inspector

    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other collider is a PlayerController
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("PlayerController detected");
            playerInRange = true;
            DialogueUI.Instance.Show(npcName, contentList);  // Show UI with NPC data
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("PlayerController exited");
            playerInRange = false;
            DialogueUI.Instance.Hide();  // Hide UI when leaving NPC range
        }
    }

    protected override void Interact()
    {
        if (playerInRange)
        {
            DialogueUI.Instance.Show(npcName, contentList);  // Interact when in range
        }
    }
}
