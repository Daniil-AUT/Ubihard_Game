using UnityEngine;

public class SkeletonFollow : MonoBehaviour
{
    public Transform player;              // Reference to the Player
    public float speed = 5f;              // NPC speed
    public float aggroDistance = 10f;     // Aggro radius

    private Animator animator;            // Animator for animation control

    void Start()
    {
        // Create Animator 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check distance between NPC and player
        float distance = Vector3.Distance(transform.position, player.position);

        // Check player is in aggro range
        bool isAggro = distance <= aggroDistance;

        // Update animation based on agrro boolean
        UpdateAnimation(isAggro);

        // Call function for NPC to face player
        FacePlayer();

        // NPC to the player if in aggro range
        if (isAggro)
        {
            MoveTowardsPlayer();
        }
    }

    // NPC movement towards player
    void MoveTowardsPlayer()
    {

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 1.5f)  
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    // NPC always facing the player
    void FacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; 

        // Rotate NPC to face the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Update animtation based on the IsWalking variable
    void UpdateAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isWalking);
        }
    }
}
