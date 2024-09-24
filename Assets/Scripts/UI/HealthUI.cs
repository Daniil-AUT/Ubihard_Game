using UnityEngine;

public class HealthBarUI: MonoBehaviour
{
    public RectTransform healthBarTransform; 
    public float maxHealth = 100f;
    public float healAmount = 10f;
    private float currentHealth;
    private float damageAmount = 10f; 

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        // Take damage if the kye P is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(damageAmount);
        }
        // Heal back damage when key O is pressed
        if (Input.GetKeyDown(KeyCode.O))
        {
            Heal(healAmount);
        }
    }
    
    // If the player takes daamge, then check if it's 0 or not
    // otherwise updater the size of the health bar (decrease)
    private void TakeDamage(float damage)
    {

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();
    }

    // If the player heals, then check if it's greater or equal to max health
    // otherwise update the size of the healthbar (increase)
    private void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // the percentage of health for ui which will be used to measure the size
        float healthPercentage = currentHealth / maxHealth;
        Vector2 size = healthBarTransform.sizeDelta;
        size.x = healthPercentage * 100; 
        healthBarTransform.sizeDelta = size;

        // turn into red colour if health is 30 or less
        if (currentHealth <= 30)
        {
            healthBarTransform.GetComponent<UnityEngine.UI.Image>().color = Color.red;
        }
        // turn into yellow colour if health is 50 or less
        else if (currentHealth <= 50)
        {
            healthBarTransform.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
        }
        // otherwsie keep it green
        else
        {
            healthBarTransform.GetComponent<UnityEngine.UI.Image>().color = Color.green;
        }
    }
}
