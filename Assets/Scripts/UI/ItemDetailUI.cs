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
    public Button consumeButton; 

    private ItemSO currentItem; 

    // Set the template to invisible mode and add the button listener
    private void Start()
    {
        propertyTemplate.SetActive(false);
        consumeButton.onClick.AddListener(ConsumeItem);
    }

    // Update the UI of a bag
    public void UpdateItemDetailUI(ItemSO itemSO)
    {
        currentItem = itemSO; 
        iconImage.sprite = itemSO.icon;
        nameText.text = itemSO.name;
        descriptionText.text = itemSO.description;

        // Delete the selected item from a Grid
        foreach (Transform child in propertyGrid.transform)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }

        // When certain ItemSO is picked, assign it the properties based on type
        foreach (ItemProperty property in itemSO.propertyList)
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

            // Re-adjust the grid template after the item is removed
            GameObject go = GameObject.Instantiate(propertyTemplate);
            go.SetActive(true);
            go.transform.SetParent(propertyGrid.transform, false);
            go.transform.Find("Property").GetComponent<TextMeshProUGUI>().text = propertyStr;
        }
    }

    // Check if the item is valid (exists), and consume it using the BagUI method
    private void ConsumeItem()
    {
        if (currentItem != null)
        {
            BagUI.Instance.ConsumeItem(currentItem);
            gameObject.SetActive(false);
        }
    }
}