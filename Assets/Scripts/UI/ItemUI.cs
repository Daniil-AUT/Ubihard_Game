using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    public void InitItem(Sprite IconSprite, string name)
    {
        iconImage.sprite = IconSprite;
        nameText.text = name;
    }


}
