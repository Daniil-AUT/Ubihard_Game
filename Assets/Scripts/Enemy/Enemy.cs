using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int HP = 100;
<<<<<<< HEAD
    public float speed = 5f;
    public float aggroDistance = 10f;
    public float restTime = 2f;

    protected NavMeshAgent enemyAgent;
    protected float restTimer = 0f;
    protected bool isPlayerInZone = false;
    protected Transform player;
    protected Animator animator;

    private enum EnemyState
=======
    public float detectionRange = 5.0f;
    private GameObject player;
    public enum EnemyState
>>>>>>> parent of 7e178b1 (Merge branch 'main' into Andy's)
    {
        NormalState,
        FightingState,
        MovingState,
        RestingState
    }

    private EnemyState currentState = EnemyState.NormalState;
    private EnemyState childState = EnemyState.RestingState;
    private NavMeshAgent enemyAgent;

    public float restTime = 2;
    private float restTimer = 0;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
<<<<<<< HEAD
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player object not found in the scene.");
        }
=======
        player = GameObject.FindWithTag("Player");
>>>>>>> parent of 7e178b1 (Merge branch 'main' into Andy's)
    }

    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(30);
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
        if (HP <= 0)
        {
            // Disable enemy's collider to prevent further interactions
            GetComponent<Collider>().enabled = false;

            // Drop items when enemy dies
            DropLoot();

            // Destroy the enemy GameObject
            Destroy(gameObject);
        }
    }

    private void DropLoot()
    {
        int count = Random.Range(1, 4); // Number of items to drop
        for (int i = 0; i < count; i++)
        {
            // Get a random item from the database
            ItemSO item = ItemDBManager.Instance.GetRandomItem();

            if (item != null && item.prefab != null)
            {
                // Instantiate the item prefab
                GameObject go = Instantiate(item.prefab, transform.position, Quaternion.identity);

                // Set tag for the item (assuming Tag.INTERACTABLE is a defined tag in your project)
                go.tag = "Interactable";

                // Add PickableObject component and configure it
                PickableObject po = go.AddComponent<PickableObject>();
                po.itemSO = item;

                // Optionally, add the item to the player's inventory here if desired
                // This step is usually handled when the player picks up the item.
            }
        }
    }
}
