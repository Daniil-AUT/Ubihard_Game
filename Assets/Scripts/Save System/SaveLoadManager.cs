using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

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

        // Save player health and position
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            saveData.playerHealth = player.currentHealth;

            // Save player position
            Vector3 playerPosition = player.transform.position;
            saveData.playerPosition[0] = playerPosition.x;
            saveData.playerPosition[1] = playerPosition.y;
            saveData.playerPosition[2] = playerPosition.z;
        }
        else
        {
            Console.WriteLine("Object Player doesn't exist");
        }

        // Save inventory items
        if (InventoryManager.Instance != null)
        {
            saveData.inventoryItems = InventoryManager.Instance.GetSaveData();
        }
        else
        {
            Console.WriteLine("Inventory Manager failed to loaded");
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
                
                // Load player data (health and position)
                GameObject player = GameObject.FindWithTag("Player");  // Find player using tag
                if (player != null)
                {
                    Player playerScript = player.GetComponent<Player>();  // Access the Player script

                    // Load player health
                    playerScript.currentHealth = saveData.playerHealth;
                    playerScript.healthBar.SetHealth(saveData.playerHealth);

                    // Create the new position Vector3
                    Vector3 newPosition = new Vector3(
                        saveData.playerPosition[0],
                        saveData.playerPosition[1],
                        saveData.playerPosition[2]
                    );


                    // If the camera is a child of the player, it will move with the player
                    // If not, find the main camera and move it too
                    Camera mainCamera = Camera.main;
                    if (mainCamera != null && !mainCamera.transform.IsChildOf(player.transform))
                    {
                        mainCamera.transform.position = newPosition + mainCamera.transform.localPosition;
                    }

                    Debug.Log($"Player and camera teleported to position: {newPosition}");
                }
                else 
                {
                    Console.WriteLine("Object Player doesn't exist");
                }

                // Load inventory items
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.LoadSaveData(saveData.inventoryItems);
                }
                else
                {
                    Console.WriteLine("Inventory Manager failed to loaded");
                }

                // Refresh UI
                InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
                if (inventoryUI != null)
                {
                    inventoryUI.RefreshUI();
                }
                else
                {
                    Console.WriteLine("Inventory Manager failed to loaded");
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
    public List<int> inventoryItems;
    public Vector3 position;
}