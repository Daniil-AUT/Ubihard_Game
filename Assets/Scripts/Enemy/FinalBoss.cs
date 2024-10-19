using UnityEngine;
using UnityEngine.AI;

public class FinalBoss : MonoBehaviour
{
    public int HP = 400;
    public int currencyReward = 300;
    public int expReward = 400; 

    public float detectionRange = 50.0f;
    public ItemSO itemToDrop;
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

    public float attackDistance = 2f; // Distance to trigger attack
    public float attackDamage = 20f;     // Amount of damage dealt to the player
    private float attackCooldown = 2f;    // Cooldown between attacks
    private float attackTimer = 0f;       // Timer to track cooldown

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        playerTargetLock = FindObjectOfType<PlayerTargetLock>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            enemyAgent.SetDestination(player.transform.position);
            if (distanceToPlayer <= attackDistance)
            {
                AttackPlayer();
            }
        }

        // State logic
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

        // Example damage trigger for testing
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(30);
        }
    }

    void AttackPlayer()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            Player playerStats = player.GetComponent<Player>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage);
            }
            attackTimer = 0f;
            anim.SetTrigger("Attack");
        }
    }

    Vector3 FindRandomPosition()
    {
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        return transform.position + randomDir.normalized * Random.Range(2, 5);
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
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

            if (playerTargetLock != null && playerTargetLock.currentTarget == transform)
            {
                playerTargetLock.isTargeting = false;
                playerTargetLock.currentTarget = null;
                playerTargetLock.targetIcon.gameObject.SetActive(false);
            }

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







