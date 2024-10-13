using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public int id;
    public string itemname;
    public ItemType itemType;
    public string description;
    public List<ItemProperty> propertyList;
    public Sprite icon;
    public GameObject prefab;
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
    CurrencyValue
}