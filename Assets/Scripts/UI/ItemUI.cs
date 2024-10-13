using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public ItemSO ItemSO { get; private set; } 

    public void InitItem(ItemSO itemSO)
    {
        iconImage.sprite = itemSO.icon;
        nameText.text = itemSO.name;
        this.ItemSO = itemSO;
    }

    // When clicked, update the item by consuming it
    public void OnClick()
    {
        BagUI.Instance.OnItemClick(ItemSO);
        BagUI.Instance.ConsumeSpecificItem(ItemSO); 
    }
}
