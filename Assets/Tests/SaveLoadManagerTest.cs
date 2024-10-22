using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManagerTests
{
    private DummySaveLoadManager dummySaveLoadManager;

    [SetUp]
    public void Setup()
    {
        dummySaveLoadManager = new DummySaveLoadManager();
    }

    [TearDown]
    public void TearDown()
    {
        dummySaveLoadManager.ClearSave();
    }

    [Test]
    public void SaveGame_SavePlayerData()
    {
        float playerHealth = 50f;
        int playerCurrency = 200;
        List<int> inventoryItemIDs = new List<int> { 1 };

        // acting
        dummySaveLoadManager.SaveGame(playerHealth, playerCurrency, inventoryItemIDs);

        // asserting
        Assert.IsTrue(File.Exists(dummySaveLoadManager.SavePath), "Save file was not created.");

        SaveData savedData = dummySaveLoadManager.LoadGame();

        Assert.AreEqual(50f, savedData.playerHealth, "Player health was not saved correctly");
        Assert.AreEqual(200, savedData.playerCurrency, "Player currency was not saved correctly");
        Assert.AreEqual(1, savedData.inventoryItemIDs.Count, "Inventory items were not saved correctly");
        Assert.AreEqual(1, savedData.inventoryItemIDs[0], "Saved item ID does not match expected value"); 
    }

    // Test for checking if loading returns null when no file exists
    [Test]
    public void LoadGame_CheckFileExists()
    {
        // acting
        SaveData savedData = dummySaveLoadManager.LoadGame();

        // asserting
        Assert.IsNull(savedData, "Loading should return null if no save file exists."); 
    }

    // Test for incorrectly saving player currency
    [Test]
    public void SaveGame_SaveCurrency()
    {
        // define player data
        float playerHealth = 50f;
        int playerCurrency = 200; 
        List<int> inventoryItemIDs = new List<int> { 1 };

        // acting
        dummySaveLoadManager.SaveGame(playerHealth, playerCurrency, inventoryItemIDs);

        // read the saved data
        SaveData savedData = dummySaveLoadManager.LoadGame();

        // asserting
        Assert.AreEqual(200, savedData.playerCurrency, "Expected player currency is incorrect.");
    }

    private class DummySaveLoadManager
    {
        public string SavePath => "gamesave.dat"; // Path to the save file

        public void SaveGame(float playerHealth, int playerCurrency, List<int> inventoryItemIDs)
        {
            SaveData saveData = new SaveData
            {
                playerHealth = playerHealth,
                playerCurrency = playerCurrency,
                inventoryItemIDs = inventoryItemIDs
            };

            // Serialize and save data
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(SavePath, FileMode.Create))
            {
                formatter.Serialize(stream, saveData);
            }
        }

        public SaveData LoadGame()
        {
            if (File.Exists(SavePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(SavePath, FileMode.Open))
                {
                    return formatter.Deserialize(stream) as SaveData;
                }
            }
            else
            {
                return null; 
            }
        }

        public void ClearSave()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public float playerHealth;
        public int playerCurrency;
        public List<int> inventoryItemIDs;
    }
}
