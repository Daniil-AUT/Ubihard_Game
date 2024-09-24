using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image skeletonKeyIcon;
    public Image mpPotionIcon;
    public Image hpPotionIcon;

    [SerializeField] private ItemSO skeletonKeyItem;
    [SerializeField] private ItemSO mpPotionItem;
    [SerializeField] private ItemSO hpPotionItem;

    private void Start()
    {
        // Initially hide all icons
        skeletonKeyIcon.gameObject.SetActive(false);
        mpPotionIcon.gameObject.SetActive(false);
        hpPotionIcon.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateItemVisibility();
    }

    private void UpdateItemVisibility()
    {
        skeletonKeyIcon.gameObject.SetActive(InventoryManager.Instance.HasItem(skeletonKeyItem));
        mpPotionIcon.gameObject.SetActive(InventoryManager.Instance.HasItem(mpPotionItem));
        hpPotionIcon.gameObject.SetActive(InventoryManager.Instance.HasItem(hpPotionItem));
    }
}