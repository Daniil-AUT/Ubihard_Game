using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int HP = 100;
   
    public int currencyReward = 20;

    public float detectionRange = 5.0f;
    private GameObject player;
    public enum EnemyState
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
        player = GameObject.FindWithTag("Player");
    }
    void Update()
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

            // Give currency to player when enemy dies
            Player PlayerStat = FindObjectOfType<Player>();
            if (PlayerStat != null)
            {
                PlayerStat.AddCurrency(currencyReward);
            }

            // Drop items when enemy dies
            DropLoot();

            // Destroy the enemy GameObject
            Destroy(gameObject);
        }
    }
    virtual protected void DropLoot()
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