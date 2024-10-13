using UnityEngine;
using System;
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

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        // Save player health
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            saveData.playerHealth = player.currentHealth;
            // Save player currency
            saveData.playerCurrency = player.currentCurrency;
        }
        else
        {
            Debug.LogError("Object Player doesn't exist");
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
                GameObject player = GameObject.FindWithTag("Player");  // Find player using tag
                if (player != null)
                {
                    Player playerScript = player.GetComponent<Player>();  // Access the Player script

                    // Load player health
                    playerScript.currentHealth = saveData.playerHealth;
                    playerScript.healthBar.SetHealth(saveData.playerHealth);
                    // Load player currency
                    playerScript.currentCurrency = saveData.playerCurrency;
                    playerScript.UpdateCurrencyUI(); // Update UI to reflect loaded currency
                }
                else
                {
                    Debug.LogError("Object Player doesn't exist");
                }
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
    public int playerCurrency; // Save player's currency
}
