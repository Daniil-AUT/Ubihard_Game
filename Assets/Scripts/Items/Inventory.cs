using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSO> items = new List<ItemSO>(); // List to hold inventory items

    // Method to add an item to the inventory
    public void AddItem(ItemSO item)
    {
        if (item != null)
        {
            items.Add(item);
            Debug.Log($"Added {item.name} to inventory.");
        }
        else
        {
            Debug.LogWarning("Attempted to add a null item to the inventory.");
        }
    }
}
