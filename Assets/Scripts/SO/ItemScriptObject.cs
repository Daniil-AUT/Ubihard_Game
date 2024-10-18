using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public int id;
    public string itemname;
    public ItemType itemType;
    public string description;
    public List<ItemProperty> propertyList;
    public Sprite icon;
    public GameObject prefab;
    public int cost;
}

public enum ItemType
{
    Weapon,
    Consumable,
    Currency
}

[Serializable]
public class ItemProperty
{
    public ItemPropertyType propertytype;
    public int value;
}

public enum ItemPropertyType
{
    HP,
    MP,
    Energy,
    Speed,
    Attack,
    Defence,
    CurrencyValue, //value of item sold
    ItemCost //cost of item purchsed
}