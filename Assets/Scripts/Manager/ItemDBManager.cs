using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDBManager : MonoBehaviour
{
    public static ItemDBManager Instance { get; private set; }
    public ItemDBSO itemDB;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    public ItemSO GetRandomItem()
    {
        int randomIndex = Random.Range(0, itemDB.itemlist.Count);
        return itemDB.itemlist[randomIndex];
    }
}
