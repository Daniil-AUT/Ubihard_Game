using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBar healthBar;  // Reference to the HealthBar script
    public float playerHealth = 100f; // Player's starting health

    void Start()
    {
        // Initialize the health bar
        healthBar.maxHealth = playerHealth;
        healthBar.CurrentHealth = playerHealth; // Initialize health bar to match starting health
    }

    void Update()
    {
        // Check if the 'Z' key is pressed to inflict damage
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(5f);  // Inflict 5 damage
        }
    }

    // Method to take damage and update health
    void TakeDamage(float damage)
    {
        playerHealth -= damage; // Reduce player's health
        if (playerHealth < 0)   // Clamp health to not go below 0
        {
            playerHealth = 0;
        }
        healthBar.CurrentHealth = playerHealth; // Update health bar
    }
}
