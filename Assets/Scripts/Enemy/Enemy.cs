using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //enemy profile:
    public int HP = 100;


    //State Machine:
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
    // Start is called before the first frame update
    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
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
            GetComponent<Collider>().enabled = false;
            int count = Random.Range(1, 4);
            for (int i = 0; i < count; i++)
            {
                ItemSO item = ItemDBManager.Instance.GetRandomItem();
                GameObject go = GameObject.Instantiate(item.prefab, transform.position, Quaternion.identity);
                go.tag = Tag.INTERACTABLE;

                PickableObject po = go.AddComponent<PickableObject>();
                po.itemSO = item;
            }

            Destroy(this.gameObject);
        }
    }
}
