using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    public Dictionary<PropertyType, List<Property>> propertyDict;
    public int hp = 100;
    public int energy = 100;
    public int mp = 100;

    // Start is called before the first frame update
    void Start()
    {
        propertyDict = new Dictionary<PropertyType, List<Property>>();
        propertyDict.Add(PropertyType.HP, new List<Property>());
        propertyDict.Add(PropertyType.MP, new List<Property>());
        propertyDict.Add(PropertyType.Energy, new List<Property>());
        propertyDict.Add(PropertyType.Speed, new List<Property>());
        propertyDict.Add(PropertyType.Attack, new List<Property>());
        propertyDict.Add(PropertyType.Defence, new List<Property>());
        propertyDict.Add(PropertyType.CurrencyValue, new List<Property>());

        AddProperty(PropertyType.Defence, 100);
        AddProperty(PropertyType.Speed, 10);
        AddProperty(PropertyType.Attack, 30);
        AddProperty(PropertyType.CurrencyValue, 13);
    }
    public void UseConsumable(ItemSO itemSO)
    {
        foreach (Property p in itemSO.propertyList)
        {
            AddProperty(p.propertytype, p.value);
        }
    }
    // Update is called once per frame
    public void AddProperty(PropertyType pt, int value)
    {
        switch (pt)
        { 
            case PropertyType.HP:
                hp += value;
                return;
            case PropertyType.MP:
                energy += value;
                break;
            case PropertyType.Energy:
                energy += value;
                break;
        }
        propertyDict.TryGetValue(pt, out List<Property> list);
        list.Add(new Property(pt, value));
    }

    public void RemoveProperty(PropertyType pt, int value)
    {
        switch (pt)
        {
            case PropertyType.HP:
                hp -= value;
                return;
            case PropertyType.MP:
                energy -= value;
                break;
            case PropertyType.Energy:
                energy -= value;
                break;
        }
        propertyDict.TryGetValue(pt, out List<Property> list);
        list.Remove(list.Find(x => x.value == value));

    }
}
