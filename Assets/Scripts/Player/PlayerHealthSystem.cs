using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBar healthBar;  
    public float playerHealth = 100f; 
    private bool isInvincible = false; 

    void Start()
    {
        healthBar.maxHealth = playerHealth;
        healthBar.CurrentHealth = playerHealth;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(5f);
        }
    }

    // take damage based on whether the player is invincible or not
    void TakeDamage(float damage)
    {
        if (!isInvincible) 
        {
        // take 5 damage if not and update health after
            playerHealth -= damage; 
            if (playerHealth < 0)   
            {
                playerHealth = 0;
            }
            healthBar.CurrentHealth = playerHealth;
        }
    }
    // set the player to be invincible based on certain condition
    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }
}
