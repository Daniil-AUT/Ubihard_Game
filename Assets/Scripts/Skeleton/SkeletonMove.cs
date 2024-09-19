using UnityEngine;

public class SkeletonFollow : MonoBehaviour
{
    public Transform player; 
    public float speed = 5f;           
    public float aggroDistance = 10f;     

    // call the animator class to triigger animation
    private Animator animator;  

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // difference between skelton and player position
        float distance = Vector3.Distance(transform.position, player.position);
        bool isAggro = distance <= aggroDistance;
        UpdateAnimation(isAggro);
        FacePlayer();

        // Check if the player is inside the aggro range of the npc 
        if (isAggro)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // get the distance between the player and the skeleon
        float distance = Vector3.Distance(transform.position, player.position);
        
        // check if skeleton is away from the player and if he's far enough then the skelton should move towards the player
        if (distance > 1.5f)  
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    // The code is used for the skeleton to always face the player when looking towards hijm
    void FacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; 

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // if the skeleton is in the aggro range, it will set the parameter to true, otherwise it will be false
    void UpdateAnimation(bool isWalking)
    {
        if (animator != null)
        {
            // the animation will play according to whether it's true or not
            animator.SetBool("IsWalking", isWalking);
        }
    }
}
