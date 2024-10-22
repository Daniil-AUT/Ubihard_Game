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
            Destroy(child.gameObject);
        }
    }

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

    public void Show()
    {
        uiGameObject.SetActive(true);
        EnableCursor(true);
    }

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
    public void ConsumeSpecificItem(ItemSO itemSO)
    {
        if (itemSO == null)
        {
            Debug.LogWarning("Cannot consume a null item.");
            return;
        }

        // Check if the item is in the inventory
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
            Debug.Log($"Consumed item: {itemSO.itemname}");

            // Apply the item effect to the player
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.ApplyItemEffect(itemSO);

                // Check if the item is an MS potion
                if (itemSO.id == 2) 
                {
                    PlayerController playerController = player.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        float speedBoostAmount = Random.Range(5f, 10f); 
                        playerController.ApplySpeedBoost(speedBoostAmount);
                    }
                }
            }
            else
            {
                Debug.LogError("Player not found in the scene.");
            }
        }
        else
        {
            Debug.LogWarning($"Item {itemSO.itemname} not found in inventory.");
        }
    }


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
    public void AddItemToBag(ItemSO itemSO)
    {
        if (itemSO != null && itemSO.id >= 1 && itemSO.id <= 3)
        {
            inventory.Add(itemSO);
            GameObject itemGo = GameObject.Instantiate(itemPrefab);
            itemGo.transform.SetParent(content.transform, false);
            ItemUI itemUI = itemGo.GetComponent<ItemUI>();
            itemUI.InitItem(itemSO);
        }
        else
        {
            Debug.LogWarning($"Item ID {itemSO.id} is not for BagUI.");
        }
    }

    public void ConsumeItem(ItemSO itemSO)
    {
        if (itemSO == null)
        {
            Debug.LogWarning("Cannot consume a null item.");
            return;
        }

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
            Debug.Log($"Consumed item: {itemSO.itemname}");

            // Apply the item effect to the player
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.ApplyItemEffect(itemSO);
            }
            else
            {
                Debug.LogError("Player not found in the scene.");
            }
        }
        else
        {
            Debug.LogWarning($"Item {itemSO.itemname} not found in inventory.");
        }
    }
}
