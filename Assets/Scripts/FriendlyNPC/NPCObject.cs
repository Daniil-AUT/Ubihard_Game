using UnityEngine;
// use the interactable object class to swee if the player is inside the radius
public class NPCObject : NPCInteractableObject
{
    // fill in the details when in the inspector
    public string npcName;  
    public string[] contentList;  
    private bool playerInRange = false;

    // if the player collides with an npc/their zone then show log it and show ui
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("player enetered the radius");
            playerInRange = true;
            DialogueUI.Instance.Show(npcName, contentList);  
        }
    }

    // Use the Player Controller script see if the player is outside the radius to remove ui
    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("player exited");
            playerInRange = false;
            DialogueUI.Instance.Hide();  
        }
    }

    protected override void Interact()
    {
        if (playerInRange)
        {
            DialogueUI.Instance.Show(npcName, contentList);  
        }
    }
}
