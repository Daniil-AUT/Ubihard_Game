using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopkeeperUI : MonoBehaviour
{
    public static ShopkeeperUI Instance { get; private set;}
    
    public GameObject ShopUI;
    public GameObject BuyMenuUI;
    public GameObject SellMenuUI;
    public SellMenu sellMenu; // Reference to the SellMenu script


    public Button buyButton;
    public Button sellButton;
    public Button leaveButton;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClick);
        sellButton.onClick.AddListener(OnSellButtonClick);
        leaveButton.onClick.AddListener(OnLeaveButtonClick);
        BuyMenuUI.SetActive(false);
        SellMenuUI.SetActive(false);
        Hide();
    }

    //show ui and mouse
    public void Show()
    {
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //hide ui and mouse
    public void Hide()
    {
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; //lock cursor after closing
    }

    // buy button click
    private void OnBuyButtonClick()
    {
        // buying items from the shop
        BuyMenuUI.SetActive(true);
        SellMenuUI.SetActive(false);
        Debug.Log("Buy clicked");

        // Add item purchasing logic here...
    }

    //sell button click
    private void OnSellButtonClick()
    {
        //handle selling items to the shop
        SellMenuUI.SetActive(true);
        BuyMenuUI.SetActive(false);
        Debug.Log("Sell clicked");
        // Add item selling logic here...
        if (sellMenu != null)
        {
            sellMenu.UpdateSellMenu(); // Update to show the player's inventory
        }
        else
        {
            Debug.LogWarning("SellMenu reference is not assigned.");
        }
    }

    //;leave button click
    private void OnLeaveButtonClick()
    {
        Hide(); //close the shop UI
        Debug.Log("Leave clicked");
    }
    


    public void UpdateSellMenu()
    {
        sellMenu.GetComponent<SellMenu>().UpdateSellMenu();
    }
}