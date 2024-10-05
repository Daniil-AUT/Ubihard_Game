using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public ItemSO itemSO; // Reference to the ItemSO scriptable object

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to an object with the PlayerController component
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // Debug log to confirm collision detection
            Debug.Log("Item collided with player.");

            // Add item to the player's inventory
            PickupItem(playerController.gameObject);
        }
    }

    void PickupItem(GameObject player)
    {
        // Example: Adding the item to the player's inventory
        InventoryManager inventory = InventoryManager.Instance;
        if (inventory != null)
        {
            inventory.AddItem(itemSO);
            // Optionally, you might want to play a sound or animation here
            Destroy(gameObject); // Destroy the item after picking up
        }
        else
        {
            Debug.LogWarning("InventoryManagerin instance not found.");
        }
    }
}
