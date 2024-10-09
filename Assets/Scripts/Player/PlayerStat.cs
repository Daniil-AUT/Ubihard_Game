using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;
    private bool isInvincible = false;

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

        // Add save and load functionality
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveLoadManager.Instance.SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            SaveLoadManager.Instance.LoadGame();
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
}