using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPUI : MonoBehaviour
{
    public TMP_Text xpText;      
    public TMP_Text levelText;   
    public Slider xpSlider;      

    private void Start()
    {
        UpdateXPDisplay(0, 100, 1); 
    }

    public void UpdateXPDisplay(int currentEXP, int expToNextLevel, int currentLevel)
    {
        // Update the XP text display
        if (xpText != null)
        {
            xpText.text = $"EXP: {currentEXP} / {expToNextLevel}";
        }
        else
        {
            Debug.LogError("XP Text UI element is not assigned.");
        }

        // Update the level text display
        if (levelText != null)
        {
            levelText.text = $"Level: {currentLevel}"; 
        }
        else
        {
            Debug.LogError("Level Text UI element is not assigned.");
        }

        // Update the slider value smoothly
        if (xpSlider != null)
        {
            xpSlider.value = Mathf.Clamp01((float)currentEXP / expToNextLevel); 
        }
        else
        {
            Debug.LogError("XP Slider UI element is not assigned.");
        }
    }
}
