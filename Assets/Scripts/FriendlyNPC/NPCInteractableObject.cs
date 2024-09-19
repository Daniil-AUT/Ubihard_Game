using UnityEngine;
using UnityEngine.AI;

public class NPCInteractableObject : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    private bool haveInteracted = false;

    public void OnClick(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;
        playerAgent.stoppingDistance = 2;
        playerAgent.SetDestination(transform.position);
        haveInteracted = false;
    }

    private void Update()
    {
        // if the player is within the range of npc, then they should be able to interact
        if (playerAgent != null && !playerAgent.pathPending && !haveInteracted)
        {
            if (playerAgent.remainingDistance <= 2)
            {
                Interact();
                haveInteracted = true;
            }
        }
    }

    protected virtual void Interact()
    {
        Debug.Log("Player inteacting with npc");
    }
}
