using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public Transform player;              
    public float speed = 5f;              
    public float aggroDistance = 10f;     
    private Animator animator;            

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        bool isAggro = distance <= aggroDistance;
        UpdateAnimation(isAggro);
        FacePlayer();
        
        if (isAggro)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 1.5f)  
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void FacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;  
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); 
    }

    void UpdateAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isWalking);
        }
    }
}
