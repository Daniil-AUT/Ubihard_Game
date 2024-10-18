using System.Collections;
using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public int level = 1;
    public int currentEXP = 0;
    public int expToNextLevel = 100;
    public int healthIncreasePerLevel = 10;
    public int attackDamageIncreasePerLevel = 3;
    public int defenseIncreasePerLevel = 1;

    private Player player;
    private XPUI xpUI;

    private void Start()
    {
        player = GetComponent<Player>();
        xpUI = FindObjectOfType<XPUI>();
        UpdateXPUI();
    }

    public void AddEXP(int amount)
    {
        currentEXP += amount;
        Debug.Log($"Added {amount} EXP. Current EXP: {currentEXP}");

        CheckLevelUp();
        UpdateXPUI();
    }

    private void CheckLevelUp()
    {
        while (currentEXP >= expToNextLevel)
        {
            currentEXP -= expToNextLevel;
            level++;
            expToNextLevel = Mathf.FloorToInt(expToNextLevel * 1.5f);
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // Increase player's stats upon leveling up
        player.maxHealth += healthIncreasePerLevel;
        player.attackDamage += attackDamageIncreasePerLevel;
        player.defense += defenseIncreasePerLevel;
        player.currentHealth = player.maxHealth;
        player.healthBar.SetMaxHealth(player.maxHealth);

        Debug.Log($"Level Up! New Level: {level}. Max Health: {player.maxHealth}, Attack Damage: {player.attackDamage}, Defense: {player.defense}");
    }

    public void UpdateXPUI() // Made this public
    {
        if (xpUI != null)
        {
            xpUI.UpdateXPDisplay(currentEXP, expToNextLevel, level);
        }
        else
        {
            Debug.LogError("XPUI not found");
        }
    }
}
