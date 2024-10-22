using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthBar : MonoBehaviour
{
    public void SetMaxHealth(int health) { }
}

public class XPUI : MonoBehaviour
{
    public void UpdateXPDisplay(int currentEXP, int expToNextLevel, int level) { }
}

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int attackDamage = 10;
    public int defense = 5;
    public HealthBar healthBar;

    public void Awake()
    {
        currentHealth = maxHealth;
    }
}

public class PlayerXP : MonoBehaviour
{
    public int level = 1;
    public int currentEXP = 0;
    public int expToNextLevel = 100;
    public int healthIncreasePerLevel = 10;
    public int attackDamageIncreasePerLevel = 3;
    public int defenseIncreasePerLevel = 1;

    [HideInInspector]
    public Player player;
    [HideInInspector]
    public XPUI xpUI;

    public void Initialize(Player p, XPUI ui)
    {
        player = p;
        xpUI = ui;
        UpdateXPUI();
    }

    public void AddEXP(int amount)
    {
        if (player == null)
        {
            Debug.LogError("Player reference is null in AddEXP");
            return;
        }

        currentEXP += amount;
        CheckLevelUp();
        UpdateXPUI();
    }

    private void CheckLevelUp()
    {
        if (player == null) return;

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
        if (player == null) return;

        player.maxHealth += healthIncreasePerLevel;
        player.attackDamage += attackDamageIncreasePerLevel;
        player.defense += defenseIncreasePerLevel;
        player.currentHealth = player.maxHealth;
        
        if (player.healthBar != null)
        {
            player.healthBar.SetMaxHealth(player.maxHealth);
        }
    }

    private void UpdateXPUI()
    {
        if (xpUI != null)
        {
            xpUI.UpdateXPDisplay(currentEXP, expToNextLevel, level);
        }
    }
}

[TestFixture]
public class PlayerXPTests
{
    private GameObject gameObject;
    private PlayerXP playerXP;
    private Player player;
    private XPUI xpUI;
    private HealthBar healthBar;

    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject("TestObject");

        player = gameObject.AddComponent<Player>();
        healthBar = gameObject.AddComponent<HealthBar>();
        xpUI = gameObject.AddComponent<XPUI>();
        playerXP = gameObject.AddComponent<PlayerXP>();

        player.healthBar = healthBar;
        playerXP.Initialize(player, xpUI);

        // Reset initial values
        player.maxHealth = 100;
        player.currentHealth = 100;
        player.attackDamage = 10;
        player.defense = 5;
        
        playerXP.level = 1;
        playerXP.currentEXP = 0;
        playerXP.expToNextLevel = 100;
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up
        if (gameObject != null)
        {
            Object.DestroyImmediate(gameObject);
        }
    }

    [UnityTest]
    public IEnumerator AcceptanceTest_LevelUp_OnFinalEnemyKill()
    {
        Assert.AreEqual(1, playerXP.level, "Initial level should be 1");
        Assert.AreEqual(100, player.maxHealth, "Initial health should be 100");

        // act
        playerXP.AddEXP(100);
        
        yield return null;

        // assert
        Assert.AreEqual(2, playerXP.level, "Player should be at level 2 after leveling up.");
        Assert.AreEqual(0, playerXP.currentEXP, "Current EXP should be reset to 0 after leveling up.");
        Assert.AreEqual(110, player.maxHealth, "Max health should have increased to 110 after leveling up.");
    }

    [UnityTest]
    public IEnumerator AcceptanceTest_NewStats_AfterInteraction()
    {
        // Verify initial state
        Assert.AreEqual(10, player.attackDamage, "Initial attack damage should be 10");
        Assert.AreEqual(5, player.defense, "Initial defense should be 5");

        // act
        playerXP.AddEXP(100); // First level up
        yield return null;
        playerXP.AddEXP(150); // Second level up
        yield return null;

        // assert
        Assert.AreEqual(16, player.attackDamage, 
            "Player's attack damage should be 16 after leveling up twice.");
        Assert.AreEqual(7, player.defense, 
            "Player defense should have increased to 7 after leveling up twice.");
    }
}