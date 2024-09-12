using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public Transform player;              // Reference to the player’s transform
    public float speed = 5f;              // Speed of the NPC
    public float aggroDistance = 10f;     // Distance within which the NPC starts moving

    private Animator animator;            // Reference to the Animator component

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Calculate the distance between the NPC and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // Check if the NPC is within the aggro distance
        bool isAggro = distance <= aggroDistance;

        // Update animation based on NPC state
        UpdateAnimation(isAggro);

        // Always face the player
        FacePlayer();

        // Move the NPC towards the player if within aggro distance
        if (isAggro)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the distance between the NPC and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // Move the NPC towards the player if beyond the stopping distance
        if (distance > 1.5f)  // Adjust the stopping distance as needed
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void FacePlayer()
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;  // Keep the NPC upright (don't tilt forward/backward)

        // Rotate the NPC to face the player smoothly
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // 5f is the rotation speed
    }

    void UpdateAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isWalking);
        }
    }
}
