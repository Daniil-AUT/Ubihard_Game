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
    public List<Property> propertyList;
    public Sprite icon;
    public GameObject prefab;
    public int cost;
    public int price;
}

public enum ItemType
{
    Weapon,
    Consumable,
    Currency
}

[Serializable]
public class Property
{
    public PropertyType propertytype;
    public int value;
    public Property()
    { 
        
    }
    public Property(PropertyType propertyType, int value)
    { 
        this.propertytype = propertyType;
        this.value = value;
    }
}

public enum PropertyType
{
    HP,
    MP,
    Energy,
    Speed,
    Attack,
    Defence,
    CurrencyValue,
    Cost
}