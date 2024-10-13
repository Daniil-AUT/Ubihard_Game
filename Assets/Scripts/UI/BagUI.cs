using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    public static BagUI Instance { get; private set; }

    private GameObject uiGameObject;
    private GameObject content;
    public GameObject itemPrefab;
    private bool isShow = false;
    public ItemDetailUI itemDetailUI;

    public Button consumeFirstItemButton;

    public List<ItemSO> inventory = new List<ItemSO>(); 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        uiGameObject = transform.Find("UI").gameObject;
        content = transform.Find("UI/background/Scroll View/Viewport/Content").gameObject;
        Hide();

        if (consumeFirstItemButton != null)
        {
            consumeFirstItemButton.onClick.AddListener(ConsumeFirstItem);
        }
    }
    public void ClearInventory()
    {
        inventory.Clear();
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject); // Remove all UI elements
        }
    }

    // Open/Close the bag UI menu
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isShow)
            {
                Hide();
                isShow = false;
            }
            else
            {
                Show();
                isShow = true;
            }
        }
    }
    

    // Will display the UI menu
    public void Show()
    {
        uiGameObject.SetActive(true);
        EnableCursor(true);
    }

    // Will hide the UI menu
    public void Hide()
    {
        uiGameObject.SetActive(false);
        EnableCursor(false);
    }

    private void EnableCursor(bool enable)
    {
        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked; 
        Cursor.visible = enable; 
    }

    // Check to see if item exists and add it to the grid
    public void AddItem(ItemSO itemSO)
    {
        if (itemSO != null) 
        {
            inventory.Add(itemSO);
            GameObject itemGo = GameObject.Instantiate(itemPrefab);
            itemGo.transform.SetParent(content.transform, false);
            ItemUI itemUI = itemGo.GetComponent<ItemUI>();
            itemUI.InitItem(itemSO);
        }
        else
        {
            Debug.LogWarning("Can't add null to inventory");
        }
    }

    // Check if the item is clicked and update it
    public void OnItemClick(ItemSO itemSO)
    {
        if (itemSO != null) 
        {
            itemDetailUI.gameObject.SetActive(true);
            itemDetailUI.UpdateItemDetailUI(itemSO);
        }
        else
        {
            Debug.LogWarning("Attempted to click a null item.");
        }
    }

    // Method updated to handle null or missing items
    public void ConsumeSpecificItem(ItemSO itemSO)
    {
        if (itemSO == null)
        {
            Debug.LogWarning("Cannot consume a null item.");
            return;
        }

        if (inventory.Count == 0)
        {
            Debug.Log("Inventory is empty. No item to consume.");
            return;
        }

        // Check if the item exists in the inventory
        if (inventory.Remove(itemSO))
        {
            // Find the corresponding UI element and destroy it
            foreach (Transform child in content.transform)
            {
                ItemUI itemUI = child.GetComponent<ItemUI>();
                if (itemUI != null && itemUI.ItemSO == itemSO)
                {
                    Destroy(child.gameObject); // Remove the item from UI
                    Debug.Log($"Consumed and removed item: {itemSO.name}");
                    break;
                }
            }

            itemDetailUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Item {itemSO.name} not found in inventory.");
        }
    }

    // Check if inventory is empty and consume the item
    public void ConsumeFirstItem()
    {
        if (inventory.Count == 0) 
        {
            Debug.LogWarning("No items in inventory to consume!");
            return;
        }

        ItemSO firstItem = inventory[0];
        ConsumeItem(firstItem);
    }

    // Consume item by getting it from a list, removing it, and removing it from ui
    public void ConsumeItem(ItemSO itemSO)
    {
        if (itemSO == null)
        {
            Debug.LogWarning("Cannot consume a null item.");
            return;
        }

        // Remove from inventory and UI (for other possible consume cases)
        if (inventory.Remove(itemSO))
        {
            foreach (Transform child in content.transform)
            {
                ItemUI itemUI = child.GetComponent<ItemUI>();
                if (itemUI != null && itemUI.ItemSO == itemSO)
                {
                    Destroy(child.gameObject); 
                    break;
                }
            }
            Debug.Log($"Consumed item: {itemSO.name}");
        }
        else
        {
            Debug.LogWarning($"Item {itemSO.name} not found in inventory.");
        }
    }
}
