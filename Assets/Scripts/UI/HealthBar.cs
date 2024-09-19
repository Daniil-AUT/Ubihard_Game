using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public RectTransform fillRectTransform; 
    public float maxHealth = 100f; 
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(); 
    }

    // Needed to find a way for it to always be between 0 and 100 (may have used gen ai)
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth); 
            UpdateHealthBar(); 
        }
    }

    // update the ui of the health bar if player clicked the button
    private void UpdateHealthBar()
    {
        if (fillRectTransform != null)
        {
            float fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            Debug.Log($"Health = {fillAmount}");
            Vector2 size = fillRectTransform.sizeDelta;
            size.x = fillAmount * 100f; 
            fillRectTransform.sizeDelta = size;
            Debug.Log($"Health After: {fillRectTransform.sizeDelta}");
        }
        else
        {
            Debug.LogError("Health is not updating");
        }
    }
}
