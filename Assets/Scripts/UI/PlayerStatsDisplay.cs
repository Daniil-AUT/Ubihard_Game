using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerStatsDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI attackDamageText;
    [SerializeField] private TextMeshProUGUI movementSpeedText;


    [Header("Stats")]
    private int currentDefense;
    private int currentAttackDamage;
    private int currentMovementSpeed;

    // References to other components
    private Player playerScript;
    private PlayerController playerController;

    private void Start()
    {
        // Get references to required components
        playerScript = FindObjectOfType<Player>();
        playerController = FindObjectOfType<PlayerController>();

        if (playerScript == null || playerController == null)
        {
            Debug.LogError("Required player components not found!");
            return;
        }

        // Initialize current stats
        currentDefense = playerScript.defense;
        currentAttackDamage = playerScript.attackDamage;
        currentMovementSpeed = Mathf.RoundToInt(playerScript.movementSpeed); 

        // Initial UI update
        UpdateAllStatsDisplay();
    }

    private void Update()
    {
        if (playerController != null && currentMovementSpeed != Mathf.RoundToInt(playerController.currentSpeed))
        {
            currentMovementSpeed = Mathf.RoundToInt(playerController.currentSpeed); 
            UpdateMovementSpeedDisplay();
        }
    }

    public void UpdateDefense(int newDefense)
    {
        currentDefense = newDefense;
        UpdateDefenseDisplay();
    }

    public void UpdateAttackDamage(int newDamage)
    {
        currentAttackDamage = newDamage;
        UpdateAttackDisplay();
    }

    public void UpdateMovementSpeed(int newSpeed)
    {
        currentMovementSpeed = newSpeed;
        UpdateMovementSpeedDisplay();
    }

    private void UpdateAllStatsDisplay()
    {
        UpdateDefenseDisplay();
        UpdateAttackDisplay();
        UpdateMovementSpeedDisplay();
    }

    private void UpdateDefenseDisplay()
    {
        if (defenseText != null)
        {
            defenseText.text = $"Defense: {currentDefense:F1}";
        }
    }

    private void UpdateAttackDisplay()
    {
        if (attackDamageText != null)
        {
            attackDamageText.text = $"Attack: {currentAttackDamage:F1}";
        }
    }

    private void UpdateMovementSpeedDisplay()
    {
        if (movementSpeedText != null)
        {
            movementSpeedText.text = $"Speed: {currentMovementSpeed:F1}";
        }
    }

    // Method to temporarily modify stats (for power-ups, debuffs, etc.)
    public IEnumerator TemporaryStatModifier(string statType, int amount, int duration)
    {
        switch (statType.ToLower())
        {
            case "defense":
                int originalDefense = currentDefense;
                UpdateDefense(currentDefense + amount);
                yield return new WaitForSeconds(duration);
                UpdateDefense(originalDefense);
                break;

            case "attack":
                int originalAttack = currentAttackDamage;
                UpdateAttackDamage(currentAttackDamage + amount);
                yield return new WaitForSeconds(duration);
                UpdateAttackDamage(originalAttack);
                break;

            case "speed":
                int originalSpeed = currentMovementSpeed;
                UpdateMovementSpeed(currentMovementSpeed + amount);
                yield return new WaitForSeconds(duration);
                UpdateMovementSpeed(originalSpeed);
                break;
        }
    }

    // Method to apply permanent stat modifications (from equipment, level-ups, etc.)
    public void ApplyPermanentStatModifier(string statType, int amount)
    {
        switch (statType.ToLower())
        {
            case "defense":
                UpdateDefense(currentDefense + amount);
                break;

            case "attack":
                UpdateAttackDamage(currentAttackDamage + amount);
                break;

            case "speed":
                UpdateMovementSpeed(currentMovementSpeed + amount);
                break;
        }
    }
}
