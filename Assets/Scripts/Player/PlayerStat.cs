using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float movementSpeed = 5f;
    public HealthBar healthBar;
    private bool isInvincible = false;
    public Vector3 playerPosition;
    public int currentCurrency = 0;
    public Animator animator; // Ensure this is linked to your Animator component
    public GameOverManager gameOverManager;
    public PlayerController playerController;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }

        if (playerController == null)
        {
            Debug.LogError("PlayerController component not found on this GameObject.");
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on this GameObject.");
            }
        }
    }

    private void Update()
    {
        playerPosition = transform.position;

        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveLoadManager.Instance.SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            SaveLoadManager.Instance.LoadGame();
        }
    }

    public void Die()
    {
        // Trigger the death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        else
        {
            Debug.LogError("Animator is not set. Cannot play death animation.");
        }

        isInvincible = true;

        // Disable player controller
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Start the coroutine to handle the game over logic
        StartCoroutine(HandleDeath());
    }

    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            healthBar.SetHealth(currentHealth);
            Debug.Log($"Player took damage: {damage}. Current health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
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

    public void UpdateCurrencyUI()
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

    // Method to apply effects based on the item
    public void ApplyItemEffect(ItemSO item)
    {
        switch (item.id)
        {
            case 1: // Heal effect
                Heal(20f); // Example healing amount
                break;

            case 2: // Speed increase
                StartCoroutine(IncreaseSpeed(5f, 10f)); // Increase by 5 for 10 seconds
                break;

            case 3: // Damage reduction effect
                TakeDamage(20f);
                Debug.Log("Damage has been reduced."); // Replace this with your damage reduction logic
                break;

            default:
                Debug.Log("No effect defined for this item.");
                break;
        }
    }

    private IEnumerator HandleDeath()
    {
        float animationDuration = 2f; // Adjust this based on your death animation length
        yield return new WaitForSeconds(animationDuration);

        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
        }
        else
        {
            Debug.LogError("GameOverManager is not set. Cannot trigger game over.");
        }
    }

    private IEnumerator IncreaseSpeed(float amount, float duration)
    {
        movementSpeed += amount;
        Debug.Log($"Movement speed increased to: {movementSpeed}");
        yield return new WaitForSeconds(duration);
        movementSpeed -= amount;
        Debug.Log($"Movement speed reverted to: {movementSpeed}");
    }
}
