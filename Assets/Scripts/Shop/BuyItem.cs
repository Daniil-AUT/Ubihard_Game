using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    public Button swordButton;      // Button for the sword
    public Button potionButton;     // Button for the health potion

    public ItemSO swordItem;        // The actual sword item (Scriptable Object)
    public ItemSO potionItem;       // The actual health potion item (Scriptable Object)

    public Button backButton;

    public GameObject ItemBuyPanel;
    public GameObject ShopUI;

    private void Start()
    {
        swordButton.onClick.AddListener(() => BuyItemFunction(swordItem));
        potionButton.onClick.AddListener(() => BuyItemFunction(potionItem));

        backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnBackButtonClick()
    {
        ItemBuyPanel.SetActive(false);
    }

    // Function that handles buying items
    private void BuyItemFunction(ItemSO item)
    {
        // Assuming you have a reference to the player's stats and currency
        Player playerStats = FindObjectOfType<Player>();

        if (playerStats.currentCurrency >= item.cost)  // Check if player has enough currency
        {
            playerStats.SpendCurrency(item.cost);  // Deduct the currency
            InventoryManager.Instance.AddItem(item);  // Add item to player's inventory
            Debug.Log($"Bought {item.itemname} for {item.cost} currency.");
        }
        else
        {
            Debug.Log("Not enough currency to buy this item.");
        }
    }
}








