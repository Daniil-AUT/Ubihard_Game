using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject propertyGrid;
    public GameObject propertyTemplate;

    private ItemSO itemSO;
    private ItemUI itemUI;

    private void Start()
    {
        propertyTemplate.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void UpdateItemDetailUI(ItemSO itemSO, ItemUI itemUI)
    {
        currentItem = itemSO; 

        iconImage.sprite = itemSO.icon;
        nameText.text = itemSO.name;
        descriptionText.text = itemSO.description;

        foreach(Transform child in propertyGrid.transform)
        {
            if (child.gameObject.activeSelf)
            { 
                Destroy(child.gameObject);
            }
        }

        foreach(ItemProperty property in itemSO.propertyList)
        {
            string propertyStr = "";
            string propertyName = "";
            switch (property.propertytype)
            {
                case ItemPropertyType.Attack:
                    propertyName = "Attack + ";
                    break;
                case ItemPropertyType.Speed:
                    propertyName = "Speed + ";
                    break;
                case ItemPropertyType.HP:
                    propertyName = "HP + ";
                    break;
                case ItemPropertyType.MP:
                    propertyName = "MP + ";
                    break;
                case ItemPropertyType.Defence:
                    propertyName = "Def + ";
                    break;
            }
            propertyStr += propertyName;
            propertyStr += property.value;
            GameObject go = GameObject.Instantiate(propertyTemplate);
            go.SetActive(true);

            go.transform.SetParent(propertyGrid.transform, false);
            go.transform.Find("Property").GetComponent<TextMeshProUGUI>().text = propertyStr;
        }
    }

    public void UseButtonClick()
    {
        BagUI.Instance.OnItemUse(itemSO, itemUI);
        this.gameObject.SetActive(false);
    }
}