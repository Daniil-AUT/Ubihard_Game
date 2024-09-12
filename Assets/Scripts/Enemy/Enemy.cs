using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    public int HP = 100;

    public float detectionRange = 5.0f;
    private GameObject player;
    public enum EnemyState

    public float speed = 5f;
    public float aggroDistance = 10f;
    public float restTime = 2f;

    protected NavMeshAgent enemyAgent;
    protected float restTimer = 0f;
    protected bool isPlayerInZone = false;
    protected Transform player;
    protected Animator animator;

    private enum EnemyState
    {
        NormalState,
        FightingState,
        MovingState,
        RestingState
    }

    private EnemyState currentState = EnemyState.NormalState;
    private EnemyState childState = EnemyState.RestingState;

    private void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        
        if (player == null)
        {
            Debug.LogError("Player object not found in the scene.");
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleDamage();
        UpdateNPCBehavior();
    }

    protected virtual void HandleMovement()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            //currentState = EnemyState.FightingState;
            enemyAgent.SetDestination(player.transform.position);
        }

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
                if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
                {
                    restTimer = 0f;
                    childState = EnemyState.RestingState;
                }
            }
        }
    }

    protected virtual void HandleDamage()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(30);
        }
    }

    protected virtual Vector3 FindRandomPosition()
    {
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        return transform.position + randomDir.normalized * Random.Range(2f, 5f);
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log($"Enemy took {damage} damage. Remaining HP: {HP}");

        if (HP <= 0)
        {
            Die(); // Ensure Die is called for proper cleanup
        }
    }

    protected virtual void Die()
    {
        GetComponent<Collider>().enabled = false;
        DropLoot(); // Call DropLoot on death
        Destroy(gameObject); // Destroy the enemy
    }

    protected abstract void DropLoot(); // Abstract method to be implemented by derived classes

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    private void UpdateNPCBehavior()
    {
        if (player == null) return; // Prevent errors if player is not found

        float distance = Vector3.Distance(transform.position, player.position);
        bool isAggro = distance <= aggroDistance;

        UpdateAnimation(isAggro);

        FacePlayer();

        if (isAggro)
        {
            MoveTowardsPlayer();
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (player == null) return; // Prevent errors if player is not found

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 1.5f)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    protected virtual void FacePlayer()
    {
        if (player == null) return; // Prevent errors if player is not found

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    protected virtual void UpdateAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isWalking);
        }
    }
}
