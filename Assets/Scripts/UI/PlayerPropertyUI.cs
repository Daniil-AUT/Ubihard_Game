using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPropertyUI : MonoBehaviour
{
    public static PlayerPropertyUI Instance { get; private set; }
    private GameObject propertyGrid;
    private GameObject propertyTemplate;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        propertyGrid = transform.Find("UI/Grid").gameObject;
        propertyTemplate = transform.Find("UI/Grid/PropertyTemplate").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
