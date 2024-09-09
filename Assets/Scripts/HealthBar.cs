using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public RectTransform fillRectTransform; // The RectTransform of the health bar fill
    public float maxHealth = 100f; // Maximum health value
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(); // Initialize health bar
    }

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth); // Clamp health to valid range
            UpdateHealthBar(); // Update health bar when health changes
        }
    }

    private void UpdateHealthBar()
    {
        if (fillRectTransform != null)
        {
            // Calculate the width based on current health
            float fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            Debug.Log($"Updating HealthBar: Fill Amount = {fillAmount}");

            // Set the width of the fillRectTransform
            Vector2 size = fillRectTransform.sizeDelta;
            size.x = fillAmount * 100f; // Adjust width based on fill amount
            fillRectTransform.sizeDelta = size;

            // Optionally, adjust other parameters if needed
            Debug.Log($"HealthBar Width Updated: {fillRectTransform.sizeDelta}");
        }
        else
        {
            Debug.LogError("Fill RectTransform is not assigned in HealthBar script.");
        }
    }
}
