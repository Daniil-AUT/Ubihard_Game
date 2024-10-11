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

    private void Start()
    {
        propertyTemplate.SetActive(false);
    }

    public void UpdateItemDetailUI(ItemSO itemSO)
    {
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
            go.transform.parent = propertyGrid.transform;
            go.transform.Find("Property").GetComponent<TextMeshProUGUI>().text = propertyStr;
        }
    }
}
