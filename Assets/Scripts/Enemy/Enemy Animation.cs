using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    public bool IsRunning;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
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
    }
}
