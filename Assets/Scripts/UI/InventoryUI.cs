using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Image skeletonKeyIcon;
    public Image mpPotionIcon;
    public Image hpPotionIcon;

    public TMP_Text skeletonText;
    public TMP_Text mpPotionText;
    public TMP_Text hpPotionText;

    public PlayerController playerStat;
    
    [SerializeField] private ItemSO skeletonKeyItem;
    [SerializeField] private ItemSO mpPotionItem;
    [SerializeField] private ItemSO hpPotionItem;
    public int healValue;
    public int takeDamagelValue;

    public int moveSpeedValue;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        
        healValue = (int) hpPotionItem.propertyList[0].value; 
        takeDamagelValue = (int) mpPotionItem.propertyList[0].value;
        moveSpeedValue = (int) mpPotionItem.propertyList[0].value;

        Debug.Log($"Heal Value {healValue}");
        HideAllItems();
    }

    private void Update()
    {
        UpdateItemVisibility();
        HandleItemUsage();
    }

    private void UpdateItemVisibility()
    {
        if (InventoryManager.Instance.HasItem(skeletonKeyItem))
        {
            skeletonKeyIcon.gameObject.SetActive(true);
            skeletonText.gameObject.SetActive(true);
            skeletonText.text = GetItemCount(skeletonKeyItem).ToString(); 
        }
        else
        {
            skeletonKeyIcon.gameObject.SetActive(false);
            skeletonText.gameObject.SetActive(false);
        }

        if (InventoryManager.Instance.HasItem(mpPotionItem))
        {
            mpPotionIcon.gameObject.SetActive(true);
            mpPotionText.gameObject.SetActive(true);
            mpPotionText.text = GetItemCount(mpPotionItem).ToString(); 
        }
        else
        {
            mpPotionIcon.gameObject.SetActive(false);
            mpPotionText.gameObject.SetActive(false);
        }

        if (InventoryManager.Instance.HasItem(hpPotionItem))
        {
            hpPotionIcon.gameObject.SetActive(true);
            hpPotionText.gameObject.SetActive(true);
            hpPotionText.text = GetItemCount(hpPotionItem).ToString(); 
        }
        else
        {
            hpPotionIcon.gameObject.SetActive(false);
            hpPotionText.gameObject.SetActive(false);
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
        skeletonKeyIcon.gameObject.SetActive(false);
        mpPotionIcon.gameObject.SetActive(false);
        hpPotionIcon.gameObject.SetActive(false);
        
        skeletonText.gameObject.SetActive(false);
        mpPotionText.gameObject.SetActive(false);
        hpPotionText.gameObject.SetActive(false);
    }

    private void HandleItemUsage()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            UseHpPotion();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            UseMpPotion();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            UseSkeletonKey();
        }
    }

    private void UseHpPotion()
    {
        if (InventoryManager.Instance.HasItem(hpPotionItem))
        {
            player.Heal(healValue); 
            InventoryManager.Instance.RemoveItem(hpPotionItem);
            UpdateItemVisibility(); 
            Debug.Log($"{hpPotionItem.name} has been used.");
        }
        else
        {
            Debug.Log($"No {hpPotionItem.name} available to use.");
        }
    }

    private void UseMpPotion()
    {
        if (InventoryManager.Instance.HasItem(mpPotionItem))
        {
            playerStat.walkSpeed = playerStat.walkSpeed * moveSpeedValue;
            playerStat.sprintSpeed = playerStat.sprintSpeed * moveSpeedValue;
            InventoryManager.Instance.RemoveItem(mpPotionItem);
            UpdateItemVisibility(); 
            Debug.Log($"{mpPotionItem.name} has been used.");
        }
        else
        {
            Debug.Log($"No {mpPotionItem.name} available to use.");
        }
    }

    private void UseSkeletonKey()
    {
        if (InventoryManager.Instance.HasItem(skeletonKeyItem))
        {
            player.TakeDamage(takeDamagelValue); 
            InventoryManager.Instance.RemoveItem(skeletonKeyItem);
            UpdateItemVisibility(); 
            Debug.Log($"{skeletonKeyItem.name} has been used.");
        }
        else
        {
            Debug.Log($"No {skeletonKeyItem.name} available to use.");
        }
    }

    // Add this method to update the UI when loading a game
    public void RefreshUI()
    {
        UpdateItemVisibility();
    }
}