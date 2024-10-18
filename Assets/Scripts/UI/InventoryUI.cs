using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; } // Singleton instance

    // Update variable names to reflect new items
    public Image armorIcon;        // Changed from skeletonKeyIcon to armorIcon
    public Image damageIcon;       // Changed from mpPotionIcon to damageIcon
    public Image teddyBearIcon;    // Changed from hpPotionIcon to teddyBearIcon

    public TMP_Text armorText;     // Changed from skeletonText to armorText
    public TMP_Text damageText;     // Changed from mpPotionText to damageText
    public TMP_Text teddyBearText;  // Changed from hpPotionText to teddyBearText

    public PlayerController playerStat;

    // Updated item references to match new item types
    [SerializeField] private ItemSO armorItem;         // Changed from skeletonKeyItem to armorItem
    [SerializeField] private ItemSO damageItem;        // Changed from mpPotionItem to damageItem
    [SerializeField] private ItemSO teddyBearItem;     // Changed from hpPotionItem to teddyBearItem

    private Player player;

    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();

        // Set initial item properties
        UpdateItemProperties(armorItem, armorIcon, armorText);
        UpdateItemProperties(damageItem, damageIcon, damageText);
        UpdateItemProperties(teddyBearItem, teddyBearIcon, teddyBearText);

        HideAllItems();
        UpdateItemVisibility(); // Update the visibility at start
    }

    private void Update()
    {
        HandleItemUsage();
    }

    private void UpdateItemProperties(ItemSO item, Image icon, TMP_Text text)
    {
        // Set the icon and description from ItemSO
        if (item != null)
        {
            icon.sprite = item.icon; // Set the item icon
            text.text = item.itemname; // Set the item name for the text UI
        }
    }

    private void UpdateItemVisibility()
    {
        HideAllItems(); // Hide all items before updating visibility

        foreach (ItemSO item in InventoryManager.Instance.itemList)
        {
            if (item.id == armorItem.id) // Armor
            {
                armorIcon.gameObject.SetActive(true);
                armorText.gameObject.SetActive(true);
                armorText.text = GetItemCount(armorItem).ToString();
            }
            else if (item.id == damageItem.id) // Damage
            {
                damageIcon.gameObject.SetActive(true);
                damageText.gameObject.SetActive(true);
                damageText.text = GetItemCount(damageItem).ToString();
            }
            else if (item.id == teddyBearItem.id) // Teddy Bear
            {
                teddyBearIcon.gameObject.SetActive(true);
                teddyBearText.gameObject.SetActive(true);
                teddyBearText.text = GetItemCount(teddyBearItem).ToString();
            }
        }
    }

    private int GetItemCount(ItemSO item)
    {
        int count = 0;
        foreach (ItemSO inventoryItem in InventoryManager.Instance.itemList)
        {
            if (inventoryItem == item)
            {
                count++;
            }
        }
        return count;
    }

    private void HideAllItems()
    {
        armorIcon.gameObject.SetActive(false);
        damageIcon.gameObject.SetActive(false);
        teddyBearIcon.gameObject.SetActive(false);

        armorText.gameObject.SetActive(false);
        damageText.gameObject.SetActive(false);
        teddyBearText.gameObject.SetActive(false);
    }

    private void HandleItemUsage()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseTeddyBear(); // Changed method call to UseTeddyBear
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseDamageItem(); // Changed method call to UseDamageItem
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseArmor(); // Changed method call to UseArmor
        }
    }

    private void UseTeddyBear()
    {
        if (InventoryManager.Instance.HasItem(teddyBearItem))
        {
            player.Heal(teddyBearItem.propertyList[0].value); // Heal value from ItemSO
            InventoryManager.Instance.RemoveItem(teddyBearItem);
            UpdateItemVisibility();
            Debug.Log($"{teddyBearItem.name} has been used.");
        }
        else
        {
            Debug.Log($"No {teddyBearItem.name} available to use.");
        }
    }

    private void UseDamageItem()
    {
        if (InventoryManager.Instance.HasItem(damageItem))
        {
            playerStat.attackDamage += 5; // Adjust the damage value here
            InventoryManager.Instance.RemoveItem(damageItem);
            UpdateItemVisibility();
            Debug.Log($"{damageItem.name} has been used.");
        }
        else
        {
            Debug.Log($"No {damageItem.name} available to use.");
        }
    }

    private void UseArmor()
    {
        if (InventoryManager.Instance.HasItem(armorItem))
        {
            player.TakeDamage(-10); // Modify this logic to suit how you want to use armor
            InventoryManager.Instance.RemoveItem(armorItem);
            UpdateItemVisibility();
            Debug.Log($"{armorItem.name} has been used.");
        }
        else
        {
            Debug.Log($"No {armorItem.name} available to use.");
        }
    }

    // Add this method to update the UI when loading a game
    public void RefreshUI()
    {
        UpdateItemVisibility();
    }
}
