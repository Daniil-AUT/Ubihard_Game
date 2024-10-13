using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<ItemSO> itemList = new List<ItemSO>();
    private ItemDBSO itemDB;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadItemDatabase();
    }

    private void LoadItemDatabase()
    {
        Debug.Log("Attempting to load ItemDatabase...");
        
        string resourcesPath = Path.Combine(Application.dataPath, "Resources");
        Debug.Log($"Resources path: {resourcesPath}");
        
        if (Directory.Exists(resourcesPath))
        {
            Debug.Log("Resources folder found.");
            string[] files = Directory.GetFiles(resourcesPath, "ItemDatabase.*");
            Debug.Log($"Files in Resources folder: {string.Join(", ", files)}");
        }
        else
        {
            Debug.LogError("Resources folder not found! Please create it in your Assets folder.");
        }

        itemDB = Resources.Load<ItemDBSO>("ItemDatabase");
        if (itemDB == null)
        {
            Debug.LogError("Failed to load ItemDatabase. Make sure it exists in the Resources folder and is named correctly.");
        }
        else
        {
            Debug.Log($"ItemDatabase loaded successfully. Item count: {itemDB.itemlist.Count}");
        }
    }

    public void AddItem(ItemSO item)
    {
        itemList.Add(item);
        BagUI.Instance.AddItem(item);
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

    public List<int> GetSaveData()
    {
        List<int> itemIds = new List<int>();
        foreach (ItemSO item in itemList)
        {
            itemIds.Add(item.id);
        }
        return itemIds;
    }

    public void LoadSaveData(List<int> savedItemIds)
    {
        itemList.Clear();
        
        if (itemDB == null)
        {
            LoadItemDatabase();
            if (itemDB == null)
            {
                Debug.LogError("Failed to load ItemDatabase. Cannot load saved items.");
                return;
            }
        }

        foreach (int itemId in savedItemIds)
        {
            ItemSO item = itemDB.itemlist.Find(i => i.id == itemId);
            if (item != null)
            {
                itemList.Add(item);
            }
            else
            {
                Debug.LogWarning($"Item with id {itemId} not found in ItemDatabase.");
            }
        }

        Debug.Log($"Loaded {itemList.Count} items into inventory.");
    }
}