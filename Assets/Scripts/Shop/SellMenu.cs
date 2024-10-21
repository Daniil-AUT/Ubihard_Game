using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellMenu : MonoBehaviour
{
    public InventoryManager inventoryManager;  // Reference to the inventory manager
    public Player playerStats;                  // Reference to the player's stats (currency)
    public ShopkeeperUI shopkeeperUI;          // Reference to the UI displaying the items
    public ItemDBSO itemDatabase;               // Reference to the Item Database
    public GameObject SellMenuUI;
    public GameObject itemListContainer;        // Panel to hold item list
    public GameObject itemEntryPrefab;          // Prefab for item entry in the sell menu

    public Transform sellMenuContent;           // Make sure this is set in the inspector

    public Button backButton;

    public void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnBackButtonClick()
    {
        SellMenuUI.SetActive(false);
    }

    // Function to sell the selected item
    public void SellSelectedItem(string itemname)
    {
        ItemSO itemToSell = GetItemByName(itemname);

        if (itemToSell != null)
        {
            int itemQuantity = inventoryManager.GetItemCount(itemToSell);

            if (itemQuantity > 0)
            {
                int sellPrice = CalculateSellPrice(itemToSell);

                inventoryManager.RemoveItem(itemToSell);
                playerStats.AddCurrency(sellPrice);
                UpdateSellMenu(); // Update UI after selling

                Debug.Log(itemToSell.itemname + " sold for " + sellPrice + " coins.");
            }
            else
            {
                Debug.Log("You don't have enough of this item to sell.");
            }
        }
        else
        {
            Debug.Log("Item not found.");
        }
    }

    // Function to get ItemSO by name from the database
    private ItemSO GetItemByName(string itemname)
    {
        return itemDatabase.itemlist.Find(item => item.itemname == itemname);
    }

    // Calculate the sell price for the item
    private int CalculateSellPrice(ItemSO itemToSell)
    {
        return itemToSell.price / 2; 
    }

    private void OnEnable()
    {
        UpdateSellMenu();
    }

    // New method to update the sell menu with the player's inventory
    public void UpdateSellMenu()
    {
        // Clear previous entries in the sell menu
        foreach (Transform child in sellMenuContent)
        {
            Destroy(child.gameObject);
        }

        // Loop through player's inventory and display each item
        foreach (var item in inventoryManager.GetAllItems())
        {
            // Instantiate a new item entry UI for each item in the inventory
            GameObject itemEntry = Instantiate(itemEntryPrefab, sellMenuContent);

            // Update the UI element with item name and count
            TextMeshProUGUI itemText = itemEntry.GetComponentInChildren<TextMeshProUGUI>();
            itemText.text = item.itemname + " - " + inventoryManager.GetItemCount(item) + " available";

            // Optionally add button listeners for selling the item
            Button sellButton = itemEntry.GetComponentInChildren<Button>();
            sellButton.onClick.AddListener(() => SellSelectedItem(item.itemname)); // Pass item name
        }
    }

}


