using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class SaveLoadManager : MonoBehaviour
{
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

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        // Save player health
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            saveData.playerHealth = player.currentHealth;
        }

        // Save inventory items
        if (InventoryManager.Instance != null)
        {
            saveData.inventoryItems = InventoryManager.Instance.GetSaveData();
        }

        // Serialize and save data
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(SavePath, FileMode.Create))
        {
            formatter.Serialize(stream, saveData);
        }

        Debug.Log("Game saved successfully!");
    }

    public void LoadGame()
    {
        if (File.Exists(SavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(SavePath, FileMode.Open))
            {
                SaveData saveData = formatter.Deserialize(stream) as SaveData;

                // Load player health
                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    player.currentHealth = saveData.playerHealth;
                    player.healthBar.SetHealth(saveData.playerHealth);
                }

                // Load inventory items
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.LoadSaveData(saveData.inventoryItems);
                }

                // Refresh UI
                InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
                if (inventoryUI != null)
                {
                    inventoryUI.RefreshUI();
                }

                Debug.Log("Game loaded successfully!");
            }
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
    public List<int> inventoryItems;
}