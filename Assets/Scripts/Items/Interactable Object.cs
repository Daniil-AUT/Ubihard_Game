using UnityEngine;
using UnityEngine.AI;

public class InteractableObject : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    private bool haveInteracted = false;

    public void OnClick(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;

        playerAgent.stoppingDistance = 2;  // Stop 2 units away from the NPC.
        playerAgent.SetDestination(transform.position);
        haveInteracted = false;  // Reset interaction flag.
    }

    private void Update()
    {
        if (playerAgent != null && !playerAgent.pathPending && !haveInteracted)
        {
            if (playerAgent.remainingDistance <= playerAgent.stoppingDistance)
            {
                Interact();
                haveInteracted = true;
            }
        }
    }

    protected virtual void Interact()
    {
        Debug.Log("Interacting with Interactable Object.");
    }
}
