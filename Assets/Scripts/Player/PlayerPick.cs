using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPick : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Tag.INTERACTABLE)
        {
            PickableObject po = collision.gameObject.GetComponent<PickableObject>();

            if (po != null)
            {
                //currency
                if (po.itemSO.itemType == ItemType.Currency)
                {
                    int currencyValue = po.itemSO.propertyList.Find(prop => prop.propertytype == ItemPropertyType.CurrencyValue)?.value ?? 0;
                    GetComponent<Player>().AddCurrency(currencyValue);
                }
                else
                {
                InventoryManager.Instance.AddItem(po.itemSO);
                }

                Destroy(po.gameObject);
            }
        }
    }
}
