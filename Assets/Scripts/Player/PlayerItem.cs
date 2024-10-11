using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }
    public void UseItem(ItemSO itemSO)
    {
        switch (itemSO.itemType)
        {
            case ItemType.Weapon:
                //playerAttack.GetWeapon();
                break;
            case ItemType.Consumable:

                break;
        }
    }
}
