using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;
    private bool isInvincible = false; 

    public int currentCurrency = 0;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isInvincible) 
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        Debug.Log($"Healed for {amount} health. Current health: {currentHealth}");
    }

    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        Debug.Log($"Added {amount} of currency. Total currency: {currentCurrency}");
        UpdateCurrencyUI();
    }

    public void SpendCurrency(int amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            Debug.Log($"Spent {amount} of currency. Remaining currency: {currentCurrency}");
            UpdateCurrencyUI();
        }
        else
        {
            Debug.Log("Not enough currency to spend.");
        }
    }
    
    private void UpdateCurrencyUI()
    {
        CurrencyUI currencyUI = FindObjectOfType<CurrencyUI>();
        if (currencyUI != null)
        {
            currencyUI.UpdateCurrencyDisplay();
        }
        else
        {
            Debug.LogError("CurrencyUI not found");
        }
    }
}
