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
    }

    // take damage based on whether the player is invincible or not
    void TakeDamage(float damage)
    {
        if (!isInvincible) 
        {
            // take 5 damage if not and update health after
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }
    }
    // set the player to be invincible based on certain condition
    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }
}
