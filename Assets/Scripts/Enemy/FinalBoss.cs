using UnityEngine;
using UnityEngine.AI;

public class FinalBoss : MonoBehaviour
{
    public int HP = 300;
    public int currencyReward = 20;
    public int expReward = 300; 

    public float detectionRange = 20.0f;
    public Player playerStat;

    private GameObject player;
    private Animator anim;
    private PlayerTargetLock playerTargetLock;
    private NavMeshAgent enemyAgent;

    public enum EnemyState
    {
        NormalState,
        FightingState,
        MovingState,
        RestingState
    }

    private EnemyState currentState = EnemyState.NormalState;
    private EnemyState childState = EnemyState.RestingState;
    public float restTime = 2;
    private float restTimer = 0;
    public ItemSO itemToDrop;

    public float attackDistance = 2f; // Distance to trigger attack
    public float attackDamage = 15f;     // Amount of damage dealt to the player
    private float attackCooldown = 2f;    // Cooldown between attacks
    private float attackTimer = 0f;       // Timer to track cooldown

    // Controller fields
    private Animator controllerAnimator;
    public bool IsRunning;
    public bool IsAttacking;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        playerTargetLock = FindObjectOfType<PlayerTargetLock>();
        controllerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Handle Movement and Attacking
        HandleMovement(distanceToPlayer);
        HandleAttack(distanceToPlayer);

        // Example damage trigger for testing
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(30);
        }
    }

    void HandleMovement(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRange)
        {
            enemyAgent.SetDestination(player.transform.position);

            // Check if the boss is moving
            if (enemyAgent.velocity.sqrMagnitude > 0.01f)
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }

            controllerAnimator.SetBool("IsRunning", IsRunning);

            // Manage resting and moving states
            if (currentState == EnemyState.NormalState)
            {
                if (childState == EnemyState.RestingState)
                {
                    restTimer += Time.deltaTime;

                    if (restTimer > restTime)
                    {
                        Vector3 randomPosition = FindRandomPosition();
                        enemyAgent.SetDestination(randomPosition);
                        childState = EnemyState.MovingState;
                    }
                }
                else if (childState == EnemyState.MovingState)
                {
                    if (enemyAgent.remainingDistance <= 0)
                    {
                        restTime = 0;
                        childState = EnemyState.RestingState;
                    }
                }
            }
        }
    }

    void HandleAttack(float distanceToPlayer)
    {
        if (distanceToPlayer <= attackDistance)
        {
            IsAttacking = true;
            IsRunning = false;
            controllerAnimator.SetBool("IsAttacking", IsAttacking);
            AttackPlayer();
        }
        else
        {
            IsAttacking = false;
        }

        controllerAnimator.SetBool("IsAttacking", IsAttacking);
    }

    void AttackPlayer()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            Player playerStats = player.GetComponent<Player>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage - playerStats.defense); // Player takes damage
            }
            attackTimer = 0f; // Reset the attack timer
            anim.SetTrigger("Attack"); // Trigger attack animation if available
        }
    }

    Vector3 FindRandomPosition()
    {
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        return transform.position + randomDir.normalized * Random.Range(2, 5);
    }

    public void TakeDamage(int damage)
    {
        HP -= playerStat.attackDamage;
        Debug.Log("Enemy HP: " + HP);
        anim.SetTrigger("EnemyHit");

        if (HP <= 0)
        {
            GetComponent<Collider>().enabled = false;

            // Give currency and experience to player when enemy dies
            Player playerStats = FindObjectOfType<Player>();
            if (playerStats != null)
            {
                playerStats.AddCurrency(currencyReward);
                playerStats.GetComponent<PlayerXP>().AddEXP(expReward);
            }

            DropLoot();

            playerTargetLock.isTargeting = false;
            playerTargetLock.currentTarget = null;
            playerTargetLock.targetIcon.gameObject.SetActive(false);

            Destroy(gameObject);
        }
    }

    protected virtual void DropLoot()
    {
        if (itemToDrop != null && itemToDrop.prefab != null)
        {
            GameObject go = Instantiate(itemToDrop.prefab, transform.position, Quaternion.identity);
            go.tag = "Interactable";

            PickableObject po = go.AddComponent<PickableObject>();
            po.itemSO = itemToDrop;
        }
    }
}
