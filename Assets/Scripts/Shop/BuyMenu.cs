using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyMenu : MonoBehaviour
{
    //the items
    public Button potionButton;
    public ItemSO potionItem;

    public Button backButton;

    public GameObject BuyMenuUI;
    public GameObject ShopUI;

    private void Start()
    {
        potionButton.onClick.AddListener(() => BuyItemFunction(potionItem));
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnBackButtonClick()
    {
        BuyMenuUI.SetActive(false);
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
