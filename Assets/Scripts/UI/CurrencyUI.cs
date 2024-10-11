using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI currencyText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCurrencyDisplay();
    }

    // Update is called once per frame
    public void UpdateCurrencyDisplay()
    {
        currencyText.text = "Currency: " + FindObjectOfType<Player>().currentCurrency;
    }
}
