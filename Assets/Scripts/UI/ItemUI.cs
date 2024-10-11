using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    private ItemSO itemSO;
    public void InitItem(ItemSO itemSO)
    {
        iconImage.sprite = itemSO.icon;
        nameText.text = itemSO.name;
        this.itemSO = itemSO;
    }
    public void OnClick()
    {
        BagUI.Instance.OnItemClick(itemSO);
    }

}
