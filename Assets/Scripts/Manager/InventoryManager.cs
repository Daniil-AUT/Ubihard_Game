using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<ItemSO> itemList = new List<ItemSO>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddItem(ItemSO item)
    {
        itemList.Add(item);
        Debug.Log($"Item {item.name} added to inventory.");
    }

    public void RemoveItem(ItemSO item)
    {
        if (itemList.Contains(item))
        {
            itemList.Remove(item);
            Debug.Log($"Item {item.name} removed from inventory.");
        }
        else
        {
            Debug.Log($"Item {item.name} not found in inventory.");
        }
    }

    public bool HasItem(ItemSO item)
    {
        return itemList.Contains(item);
    }
}
