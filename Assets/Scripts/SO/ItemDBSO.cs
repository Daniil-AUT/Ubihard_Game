using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDBSO : ScriptableObject
{
    public List<ItemSO> itemlist = new List<ItemSO>();
}