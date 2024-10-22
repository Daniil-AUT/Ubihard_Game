using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public Dictionary<ItemSO, int> itemDictionary = new Dictionary<ItemSO, int>(); 
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
        if (itemDictionary.ContainsKey(item))
        {
            itemDictionary[item]++;
        }
        else
        {
            itemDictionary[item] = 1;
        }

        if (item.id >= 1 && item.id <= 3)
        {
            BagUI.Instance.AddItemToBag(item); 
        }
        else if (item.id >= 8)
        {
            BagUI.Instance.AddItemToBag(item);
        }
        else if (item.id >= 4 && item.id <= 6)
        {
            Debug.Log($"Item ID {item.id} is reserved for InventoryUI.");
            // Refresh UI to display the newly added item
            InventoryUI.Instance.RefreshUI();
        }
        else
        {
            Debug.LogWarning($"Item ID {item.id} does not fall into BagUI or InventoryUI ranges.");
        }

        Debug.Log($"Item {item.name} added to inventory.");
        
    }

    public void RemoveItem(ItemSO item)
    {
        if (itemDictionary.ContainsKey(item))
        {
            itemDictionary[item]--;
            if (itemDictionary[item] <= 0)
            {
                itemDictionary.Remove(item);
            }
            Debug.Log($"Item {item.name} removed from inventory.");
        }
        else
        {
            Debug.Log($"Item {item.name} not found in inventory.");
        }
    }

    public int GetItemCount(ItemSO item)
    {
        if (itemDictionary.TryGetValue(item, out int count))
        {
            return count;
        }
        return 0; 
    }

    public bool HasItem(ItemSO item)
    {
        return itemDictionary.ContainsKey(item);
    }

    public List<int> GetSaveData()
    {
        List<int> itemIds = new List<int>();
        foreach (var pair in itemDictionary)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                itemIds.Add(pair.Key.id);
            }
        }
        return itemIds;
    }

    public void LoadSaveData(List<int> savedItemIds)
    {
        itemDictionary.Clear();

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
                AddItem(item); 
            }
            else
            {
                Debug.LogWarning($"Item with id {itemId} not found in ItemDatabase.");
            }
        }

        Debug.Log($"Loaded {itemDictionary.Count} items into inventory.");
    }

    public List<ItemSO> GetAllItems()
    {
        return itemDictionary.Keys.ToList(); //returns list of all items
    }
}
