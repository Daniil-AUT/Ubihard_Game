using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Image[] itemSlots; // Assign the UI image slots in the Inspector

    void Start()
    {
        inventory.onItemChangedCallback += UpdateUI;
        UpdateUI(); // Initial call to update UI
    }

    void UpdateUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                itemSlots[i].sprite = inventory.items[i].icon; // Show item icon
                itemSlots[i].gameObject.SetActive(true); // Show the slot
            }
            else
            {
                itemSlots[i].gameObject.SetActive(false); // Hide empty slots
            }
        }
    }

    void Update()
    {
        // Handle item activation using keys 1, 2, 3, 4
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString())) // Check for keys 1-4
            {
                inventory.ActivateItem(i);
            }
        }
    }
}
