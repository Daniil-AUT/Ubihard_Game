using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
    public Player playerTeleport;
    private static SaveLoadManager _instance;

    public static SaveLoadManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveLoadManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("SaveLoadManager");
                    _instance = go.AddComponent<SaveLoadManager>();
                }
            }
            return _instance;
        }
    }

    private string SavePath => Path.Combine(Application.persistentDataPath, "gamesave.dat");

    // Reference to your ItemDBSO
    public ItemDBSO itemDatabase; // Drag your Item Database asset here in the Inspector

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        // Save player health
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            saveData.playerHealth = player.currentHealth;
            saveData.playerCurrency = player.currentCurrency; // Save player currency
        }
        else
        {
            Debug.LogError("Object Player doesn't exist");
        }

        // Save inventory items
        saveData.inventoryItemIDs = new List<int>();
        foreach (ItemSO item in BagUI.Instance.inventory)
        {
            saveData.inventoryItemIDs.Add(item.id); // Save the item ID
        }

        // Serialize and save data
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(SavePath, FileMode.Create))
        {
            formatter.Serialize(stream, saveData);
        }

        Debug.Log("Game saved successfully");
    }

    public void LoadGame()
    {
        if (File.Exists(SavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(SavePath, FileMode.Open))
            {
                SaveData saveData = formatter.Deserialize(stream) as SaveData;

                // Load player data (health and currency)
                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    player.currentHealth = saveData.playerHealth;
                    player.healthBar.SetHealth(saveData.playerHealth); // Update health bar UI

                    player.currentCurrency = saveData.playerCurrency;
                    player.UpdateCurrencyUI(); // Update UI to reflect loaded currency
                }
                else
                {
                    Debug.LogError("Object Player doesn't exist");
                }

                // Load inventory items
                foreach (int itemId in saveData.inventoryItemIDs)
                {
                    ItemSO item = itemDatabase.itemlist.Find(i => i.id == itemId); // Find the item using itemId
                    if (item != null)
                    {
                        BagUI.Instance.AddItem(item); // Use the existing AddItem method
                    }
                    else
                    {
                        Debug.LogWarning($"Item with ID {itemId} not found.");
                    }
                }
            }
            Debug.Log("Game loaded successfully");
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }
}

[System.Serializable]
public class SaveData
{
    public float playerHealth;
    public int playerCurrency; // Save player's currency
    public List<int> inventoryItemIDs;
}
