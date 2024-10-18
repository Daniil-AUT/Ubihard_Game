using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
    public Player playerTeleport; // Reference to your Player object
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

    public ItemDBSO itemDatabase; // Reference to your item database

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            saveData.playerHealth = player.currentHealth;
            saveData.playerCurrency = player.currentCurrency;
            saveData.playerPosition = new SerializableVector3(player.transform.position); // Convert Vector3 to SerializableVector3
        }
        else
        {
            Debug.LogError("Object Player doesn't exist");
        }

        saveData.inventoryItemIDs = new List<int>();
        foreach (ItemSO item in BagUI.Instance.inventory)
        {
            saveData.inventoryItemIDs.Add(item.id);
        }

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

            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.currentHealth = saveData.playerHealth;
                player.healthBar.SetHealth(saveData.playerHealth);
                player.currentCurrency = saveData.playerCurrency;
                player.UpdateCurrencyUI();

                // Teleport the player to the saved position
                player.TeleportToPosition(saveData.playerPosition.ToVector3()); // Call the teleport method

            }
            else
            {
                Debug.LogError("Object Player doesn't exist");
            }

            foreach (int itemId in saveData.inventoryItemIDs)
            {
                ItemSO item = itemDatabase.itemlist.Find(i => i.id == itemId);
                if (item != null)
                {
                    BagUI.Instance.AddItem(item);
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



    // New method to get the player's current position as a SerializableVector3
    public SerializableVector3 GetPlayerPosition()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            return new SerializableVector3(player.transform.position);
        }
        else
        {
            Debug.LogError("Object Player doesn't exist");
            return null; // or handle as appropriate
        }
    }
}

[System.Serializable]
public class SaveData
{
    public float playerHealth;
    public int playerCurrency;
    public SerializableVector3 playerPosition;
    public List<int> inventoryItemIDs;
}

[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;
    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    // Method to convert back to Vector3
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
