using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float movementSpeed = 5f;
    public int attackDamage = 20;
    public HealthBar healthBar;
    private bool isInvincible = false;
    public Vector3 playerPosition;
    public int currentCurrency = 0;
    public GameOverManager gameOverManager;
    public PlayerController playerController;
    private Animator animator;
    private bool isDead = false;
    private Vector3 deathPosition;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject or its children.");
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

    public void TakeDamage(float damage)
    {
        if (!isInvincible && !isDead)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            healthBar.SetHealth(currentHealth);
            Debug.Log($"Player took damage: {damage}. Current health: {currentHealth}");

            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        deathPosition = transform.position; 
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        StartCoroutine(ShowGameOverScreen());
    }

    private void LateUpdate()
    {
        if (isDead)
        {
            transform.position = deathPosition; // Lock the player's position
        }
    }

    private IEnumerator ShowGameOverScreen()
    {
        if (animator != null)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            yield return new WaitForSeconds(1f); // Default wait time if animator is not found
        }

        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
        }
        else
        {
            Debug.LogError("GameOverManager not assigned to Player script.");
        }
    }

    public void ResetPlayer()
    {
        isDead = false;
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
        transform.position = new Vector3(70.1f, 23f, 37.37f); // Respawn position
        
        playerController.enabled = true; // Re-enable movement
        
        if (animator != null)
        {
            animator.Rebind(); 
            animator.Update(0f);
        }
        playerController.ResetController();
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

    public void ApplyItemEffect(ItemSO item)
    {
        switch (item.id)
        {
            case 1: // Heal effect
                Heal(20f); 
                break;

            case 2: // Speed increase
                StartCoroutine(IncreaseSpeed(5f, 10f)); 
                break;

            case 3: // Damage reduction effect
                TakeDamage(20f);
                Debug.Log("Damage has been reduced.");
                break;

            default:
                Debug.Log("No effect defined for this item.");
                break;
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

    // New method to set player position (used when loading game)
    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
        playerPosition = newPosition;
        if (playerController != null)
        {
            playerController.ResetController();
        }
    }

    public void TeleportToPosition(Vector3 newPosition)
{
    transform.position = newPosition; // Teleport to the new position
    playerPosition = newPosition; // Update player position
    if (playerController != null)
    {
        playerController.ResetController(); // Reset the player controller if necessary
    }
}

}