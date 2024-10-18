using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    private PlayerStatsDisplay playerStatsDisplay;

    // Variables for item icons and texts
    public Image armorIcon;
    public Image damageIcon;
    public Image teddyBearIcon;

    public TMP_Text armorText;
    public TMP_Text damageText;
    public TMP_Text teddyBearText;

    public Player playerStat;

    [SerializeField] public ItemSO armorItem; // Changed to public
    [SerializeField] public ItemSO damageItem; // Changed to public
    [SerializeField] public ItemSO teddyBearItem;

    private Player player;

    // Reference to the Gift Confirmation Panel
    public GiftConfirmationPanel giftConfirmationPanel;

    private void Awake()
    {
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
        playerStatsDisplay = FindObjectOfType<PlayerStatsDisplay>();

        // Update the UI with item properties (icons, texts)
        RefreshUI();

        HideAllItems();
        UpdateItemVisibility();
    }

    private void Update()
    {
        HandleItemUsage();
    }

    private void UpdateItemProperties(ItemSO item, Image icon, TMP_Text text)
    {
        icon.sprite = item.icon;
        text.text = GetItemCount(item).ToString();
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

    private void UpdateItemVisibility()
    {
        armorIcon.gameObject.SetActive(InventoryManager.Instance.HasItem(armorItem));
        damageIcon.gameObject.SetActive(InventoryManager.Instance.HasItem(damageItem));
        teddyBearIcon.gameObject.SetActive(InventoryManager.Instance.HasItem(teddyBearItem));

        armorText.gameObject.SetActive(InventoryManager.Instance.HasItem(armorItem));
        damageText.gameObject.SetActive(InventoryManager.Instance.HasItem(damageItem));
        teddyBearText.gameObject.SetActive(InventoryManager.Instance.HasItem(teddyBearItem));
    }

    private void HandleItemUsage()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseTeddyBear();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseDamageItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseArmor();
        }
    }

    public void UseTeddyBear()
    {
        // Get the reference to the NPCObject
        NPCObject currentNPC = FindObjectOfType<NPCObject>();

        // Check if the dialogue has finished and the player is in range
        if (currentNPC != null && !currentNPC.IsPlayerInRange())
        {
            Debug.Log("Player must be in range to use the teddy bear.");
            return;
        }

        if (!DialogueUI.isDialogueFinished)
        {
            Debug.Log("Dialogue must finish before using the teddy bear.");
            return;
        }

        // Check if the player has the teddy bear item
        if (InventoryManager.Instance.HasItem(teddyBearItem))
        {
            playerStat.currentHealth += 20;
            InventoryManager.Instance.RemoveItem(teddyBearItem);
            RefreshUI();

            Debug.Log($"{teddyBearItem.name} has been used.");

            // Show the gift confirmation panel
            if (giftConfirmationPanel != null)
            {
                giftConfirmationPanel.ShowPanel();
            }
        }
        else
        {
            // Show notification panel to inform the user they need to give the teddy bear
            NotificationPanel notificationPanel = FindObjectOfType<NotificationPanel>();
            if (notificationPanel != null)
            {
                notificationPanel.ShowNotification("You need to give the teddy bear!", 2f);
            }
            Debug.Log($"No {teddyBearItem.name} available to use.");
        }
    }

    private void UseDamageItem()
    {
        if (InventoryManager.Instance.HasItem(damageItem))
        {
            playerStat.attackDamage += 5;
            InventoryManager.Instance.RemoveItem(damageItem);
            RefreshUI();

            // Update the attack damage in the UI
            playerStatsDisplay.UpdateAttackDamage(playerStat.attackDamage);

            Debug.Log($"{damageItem.name} has been used.");
        }
        else
        {
            Debug.Log($"No {damageItem.name} available to use.");
        }
    }

    public void RefreshUI()
    {
        UpdateItemProperties(armorItem, armorIcon, armorText);
        UpdateItemProperties(damageItem, damageIcon, damageText);
        UpdateItemProperties(teddyBearItem, teddyBearIcon, teddyBearText);

        UpdateItemVisibility();
    }

    private void UseArmor()
    {
        if (InventoryManager.Instance.HasItem(armorItem))
        {
            playerStat.defense += 1;
            InventoryManager.Instance.RemoveItem(armorItem);
            RefreshUI();
            playerStatsDisplay.UpdateDefense(playerStat.defense);
            Debug.Log($"{armorItem.name} has been used.");
        }
        else
        {
            Debug.Log($"No {armorItem.name} available to use.");
        }
    }

    // Method to get the count of a specific item in the inventory
    private int GetItemCount(ItemSO item)
    {
        return InventoryManager.Instance.GetItemCount(item);
    }
}
