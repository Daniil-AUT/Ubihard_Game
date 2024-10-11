using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BagUI : MonoBehaviour
{
    public static BagUI Instance { get; private set; }
    private GameObject uiGameObject;
    private GameObject content;
    public GameObject itemPrefab;
    private bool isShow = false;

    public ItemDetailUI itemDetailUI;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        uiGameObject = transform.Find("UI").gameObject;
        content = transform.Find("UI/background/Scroll View/Viewport/Content").gameObject;
        Hide();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isShow)
            {
                Hide();
                isShow = false;
            }
            else
            {
                Show();
                isShow = true;
            }
        }
    }
    public void Show()
    {
        uiGameObject.SetActive(true);
    }

    public void Hide()
    {
        uiGameObject.SetActive(false);
    }

    public void AddItem(ItemSO itemSO)
    { 
        GameObject itemGo = GameObject.Instantiate(itemPrefab);
        itemGo.transform.parent = content.transform;
        ItemUI itemUI = itemGo.GetComponent<ItemUI>();

        itemUI.InitItem(itemSO);
    }
    public void OnItemClick(ItemSO itemSO)
    { 
        itemDetailUI.UpdateItemDetailUI(itemSO);
    }
}