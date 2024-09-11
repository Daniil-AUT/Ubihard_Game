using System.Collections.Generic;
using UnityEngine;

public class ItemDBManager : MonoBehaviour
{
    public static ItemDBManager Instance { get; private set; }
    public ItemDBSO itemDB;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Keep instance across scenes
    }

    public ItemSO GetRandomItem()
    {
        if (itemDB.itemlist.Count == 0)
        {
            Debug.LogWarning("ItemDB item list is empty.");
            return null; // Handle empty list case
        }

        int randomIndex = Random.Range(0, itemDB.itemlist.Count);
        return itemDB.itemlist[randomIndex];
    }

    // Add item to the database
    public void AddItem(ItemSO item)
    {
        if (!itemDB.itemlist.Contains(item))
        {
            itemDB.itemlist.Add(item);
            Debug.Log($"Item {item.name} added to ItemDB.");
        }
        else
        {
            Debug.LogWarning($"Item {item.name} already exists in ItemDB.");
        }
    }
}
