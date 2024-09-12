using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    public bool IsRunning;
    public bool IsAttacking;
    private NavMeshAgent navMeshAgent;
    private GameObject player;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (navMeshAgent.velocity.sqrMagnitude > 0.01f)
        {
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }

        animator.SetBool("IsRunning", IsRunning);

        if (Vector3.Distance(transform.position, player.transform.position) <= 1.5f)
        {
            IsAttacking = true;
            IsRunning = false;
        }
        else
        {
            IsAttacking = false;
        }

        animator.SetBool("IsAttacking", IsAttacking);
    }
}