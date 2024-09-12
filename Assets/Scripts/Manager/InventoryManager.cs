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

    public bool HasItem(ItemSO item)
    {
        return itemList.Contains(item);
    }
}
